using ChatRoom.Core.SqlSuger;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoom.Repository.RepositoryBase.IRepositoryService
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// UnitOfWork
        /// </summary>
        ISugarUnitOfWork<SugarUnitOfWork> _unit { get; set; }

        /// <summary>
        /// 开始事务
        /// </summary>
        void BeginTran(); 

        /// <summary>
        /// 提交事务
        /// </summary>
        void CommitTran();

        /// <summary>
        /// 回滚
        /// </summary>
        void Rollback();

        /// <summary>
        /// 获取列表,带分页
        /// </summary>
        /// <param name="condi"></param>
        /// <returns></returns>
        List<TEntity> QueryPage(SearchCondition condi);
        /// <summary>
        /// 获取列表不带分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <param name="by"></param>
        /// <param name="asc"></param>
        /// <param name="top"></param>
        /// <returns></returns>
        List<TEntity> QueryList(Expression<System.Func<TEntity, bool>> where, Expression<System.Func<TEntity, object>> by = null, bool asc = false, int top = 0, string feild = null);

        /// <summary>
        /// 获取列表,不带分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <param name="by"></param>
        /// <param name="asc"></param>
        /// <param name="top"></param>
        /// <returns></returns>
        List<T> QueryList<T>(Expression<System.Func<T, bool>> where, Expression<System.Func<T, object>> by = null, bool asc = false, int top = 0, string feild = null);

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="insertColumns"></param>
        /// <returns></returns>
        bool Insert(TEntity entity, Expression<System.Func<TEntity, object>> insertColumns = null);

        /// <summary>
        /// 插入获取IdentityID
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int InsertIdentity(TEntity entity);

        /// <summary>
        /// 获取单条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <param name="feild"></param>
        /// <returns></returns>
        TEntity QueryOne(Expression<System.Func<TEntity, bool>> where, string feild = null);
        /// <summary>
        /// 更新记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="columns"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        bool Update(Expression<System.Func<TEntity, TEntity>> columns, Expression<System.Func<TEntity, bool>> expression);
        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="columns"></param>
        /// <returns></returns>
        bool Update(TEntity columns);
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        bool Delete(Expression<System.Func<TEntity, bool>> expression);
        /// <summary>
        /// 删除多条记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inValues"></param>
        /// <returns></returns>
        bool DeleteIn(Expression<System.Func<TEntity, object>> feild, List<dynamic> inValues);
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="datas"></param>
        /// <param name="procedure"></param>
        /// <param name="outParm"></param>
        /// <returns></returns>
        bool DoProcedure(List<SugarParameter> parms, string procedure);
        /// <summary>
        /// 批量插入
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="mark"></param>
        /// <returns></returns>
        bool DoBulk(List<TEntity> data);
        /// <summary>
        /// 检查是否存在
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        bool Exists(Expression<System.Func<TEntity, bool>> expression);
        /// <summary>
        /// 获取最大值
        /// </summary>
        /// <param name="feild"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        int Max(string feild, Expression<System.Func<TEntity, bool>> where = null);

        /// <summary>
        /// 获取数量
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        int GetCount(Expression<Func<TEntity, bool>> where);
          
 
    }
}
