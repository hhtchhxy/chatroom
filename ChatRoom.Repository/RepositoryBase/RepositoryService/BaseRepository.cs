using ChatRoom.Core.SqlSuger;
using ChatRoom.Repository.RepositoryBase.IRepositoryService;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoom.Repository.RepositoryBase.RepositoryService
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class, new()
    {
        public SqlSugar.ISugarUnitOfWork<SqlSugar.SugarUnitOfWork> _unit { get; set; }

        private readonly SqlSugar.ISqlSugarClient _dbBase = null;
        private SqlSugar.ISqlSugarClient _db { get { return _dbBase; } }

        public BaseRepository(SqlSugar.ISugarUnitOfWork<SqlSugar.SugarUnitOfWork> unitOfWork)
        {
            _unit = unitOfWork;
            _dbBase = unitOfWork.Db;
        }

        /// <summary>
        /// 提交事务
        /// </summary>
        public void BeginTran() { this._db.Ado.BeginTran(); }


        /// <summary>
        /// 提交事务
        /// </summary>
        public void CommitTran()
        {
            try { this._db.Ado.CommitTran(); }
            catch (System.Exception ex)
            {
                this._db.Ado.RollbackTran();
                throw new Exception($"Commit 异常：{ex.InnerException}/r{ex.Message}");
            }
        }

        /// <summary>
        /// 回滚
        /// </summary>
        public void Rollback() { this._db.Ado.RollbackTran(); }

        /// <summary>
        /// 单表带分页
        /// </summary>
        /// <param name="condi"></param>
        /// <returns></returns>
        public virtual List<TEntity> QueryPage(SearchCondition condi)
        {
            var total = 0;
            var query = _db.Queryable<TEntity>().With(SqlSugar.SqlWith.NoLock);
            if (!string.IsNullOrEmpty(condi.condi))
            {
                query.Where(condi.condi);
            }
            query.OrderBy(condi.by ?? "id desc");
            if (!string.IsNullOrEmpty(condi.feild)) { query.Select(condi.feild); }
            var d = query.ToPageList(condi.page, condi.size, ref total);
            condi.total = total;
            return d;
        }

        /// <summary>
        /// 获取列表,不带分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <param name="by"></param>
        /// <param name="asc"></param>
        /// <param name="top"></param>
        /// <returns></returns>
        public virtual List<TEntity> QueryList(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, object>> by = null, bool asc = false, int top = 0, string feild = null)
        {
            var _q = _db.Queryable<TEntity>().With(SqlSugar.SqlWith.NoLock).OrderByIF(by != null, by, asc ? SqlSugar.OrderByType.Asc : SqlSugar.OrderByType.Desc).WhereIF(where != null, where);
            if (!string.IsNullOrEmpty(feild)) { _q.Select(feild); }
            return top > 0 ? _q.Take(top).ToList() : _q.ToList();
        }

        /// <summary>
        /// 获取列表,不带分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <param name="by"></param>
        /// <param name="asc"></param>
        /// <param name="top"></param>
        /// <returns></returns>
        public List<T> QueryList<T>(Expression<Func<T, bool>> where, Expression<Func<T, object>> by = null, bool asc = false, int top = 0, string feild = null)
        {
            var _q = _db.Queryable<T>().With(SqlSugar.SqlWith.NoLock).OrderByIF(by != null, by, asc ? SqlSugar.OrderByType.Asc : SqlSugar.OrderByType.Desc).WhereIF(where != null, where);
            if (!string.IsNullOrEmpty(feild)) { _q.Select(feild); }
            return top > 0 ? _q.Take(top).ToList() : _q.ToList();
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="insertColumns"></param>
        /// <returns></returns>
        public virtual bool Insert(TEntity entity, Expression<Func<TEntity, object>> insertColumns = null)
        {
            var insert = _db.Insertable<TEntity>(entity);
            if (insertColumns == null)
            {
                return insert.ExecuteCommand() > 0;
            }
            else
            {
                return insert.InsertColumns(insertColumns).ExecuteCommand() > 0;
            }
        }

        /// <summary>
        /// 插入获取IdentityID
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual int InsertIdentity(TEntity entity)
        {
            var insert = _db.Insertable<TEntity>(entity);
            return insert.ExecuteReturnIdentity();
        }

        /// <summary>
        /// 获取单条
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <param name="feild"></param>
        /// <returns></returns>
        public virtual TEntity QueryOne(Expression<Func<TEntity, bool>> where, string feild = null)
        {
            var d = _db.Queryable<TEntity>().With(SqlSugar.SqlWith.NoLock).WhereIF(where != null, where);
            if (!string.IsNullOrEmpty(feild)) { d.Select(feild); }
            var s = d.First();
            return s;
        }
        /// <summary>
        /// 更新记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="columns"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public virtual bool Update(Expression<Func<TEntity, TEntity>> columns, Expression<Func<TEntity, bool>> expression)
        {
            var r = _db.Updateable<TEntity>().SetColumns(columns).Where(expression).ExecuteCommand();
            return r > 0;
        }
        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="columns"></param>
        /// <returns></returns>
        public virtual bool Update(TEntity columns)
        {
            var r = _db.Updateable<TEntity>(columns).ExecuteCommand();
            return r > 0;
        }
        /// <summary>
        /// 删除记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public virtual bool Delete(Expression<Func<TEntity, bool>> expression)
        {
            var r = _db.Deleteable<TEntity>().Where(expression).ExecuteCommand();
            return r > 0;
        }

        /// <summary>
        /// 删除多条记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inValues"></param>
        /// <returns></returns>
        public virtual bool DeleteIn(Expression<Func<TEntity, object>> feild, List<dynamic> inValues)
        {
            var r = _db.Deleteable<TEntity>().In(feild, inValues).ExecuteCommand();
            return r > 0;
        }

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="datas"></param>
        /// <param name="procedure"></param>
        /// <param name="outParm"></param>
        /// <returns></returns>
        public virtual bool DoProcedure(List<SqlSugar.SugarParameter> parms, string procedure)
        {
            var result = 0;
            try
            {
                result = _db.Ado.UseStoredProcedure().ExecuteCommand(procedure, parms);
            }
            catch (Exception ex)
            {
                //logger
                throw ex;
            }
            return result > 0;
        }

        /// <summary>
        /// 批量插入
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="mark"></param>
        /// <returns></returns>
        public virtual bool DoBulk(List<TEntity> data)
        {
            var r = _db.Insertable<TEntity>(data).ExecuteCommand();
            return r > 0;
        }

        /// <summary>
        /// 检查是否存在记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public virtual bool Exists(Expression<Func<TEntity, bool>> expression)
        {
            return _db.Queryable<TEntity>().With(SqlSugar.SqlWith.NoLock).Any(expression);
        }

        /// <summary>
        /// 获取最大值
        /// </summary>
        /// <param name="feild"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public virtual int Max(string feild, Expression<Func<TEntity, bool>> where = null)
        {
            int r = _db.Queryable<TEntity>().WhereIF(where != null, where).Max<int>(feild);
            return r;
        }

        /// <summary>
        /// 获取数量
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public virtual int GetCount(Expression<Func<TEntity, bool>> where)
        {
            var r = _db.Queryable<TEntity>().WhereIF(where != null, where).Count();
            return r;
        }

    }
}
