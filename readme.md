# **Overview**

1. The system currently has implemented registration and login interfaces, which provide basic security processing mechanisms such as password strength verification，password encryption using irreversible encryption for Password + random Salt, login error limit, verification code validation, IP traffic limiting, and abnormal login reminder. When the system is deployed distributed, it is necessary to store rate limit calculators and IP rules in a distributed cache such as Redis, and the API Swagger address is http://localhost:8941/index.html#/home
2. The chat service module provides chat module interface services based on SignalR, using WebSocket. Currently, it has implemented end-to-end, group chat, and broadcast interfaces, and messages can be pushed in real time. Currently, only online messages are processed, offline messages cannot be sent. Due to the large volume of chat data and high concurrency.Real-time storage records will put great pressure on the system and database. The system uses asynchronous storage, temporarily storing messages in thread-safe caches (ConcurrentQueue, Redis) when they are sent. One or more workers are started on the backend of the service to perform batch processing of data, increasing the insertion speed of data. Additionally, storage can be performed on different databases or tables based on rules such as sharding and partitioning.The ChatRoom address is http://localhost:5168/chatIndex.html. You need to open multiple windows to see the effect. Currently, users are randomly simulated and you need to start ChatRoom firstStart the ChatServerApi service and then start ChatRoomChatWeb

code structure：

  --ChatRoom.Api            			->[Webapi interface]：Provide interface services for the web end or other ports
  --ChatRoom.ChatServerApi	->[Chat Service]：Provide background interface service for chat web end
  --ChatRoom.ChatWeb        	 ->[Chat web module]：Chat module web end
  --ChatRoom.Core          		   ->[Foundation Classes]：Provide major basic functions, components or services
  --ChatRoom.Model          		->[Entity layer class library]：Business library tables, data transmission objects, etc
  --ChatRoom.Repository     	 ->[Warehouse level class library]：Provide database operations
  --ChatRoom.Service        	    ->[Service layer class library]：Process business logic and provide it to Api interface call





## 概述

1. 系统目前已实现注册、登录接口，接口提供采用提供基本的安全处理机制，如密码强度校验，密码采用不可逆加密方式对Password+随机Salt进行加密存储，登录错误次数限制，验证码验证，IP限流、异常登录提醒等，当系统分布式部署时，需要将速率限制计算器和ip规则存储到分布式缓存中如Redis， API Swagger 地址 http://localhost:8941/index.html#/home
2. 聊天服务模块，基于SignalR提供聊天模块接口服务，采用WebSocket方式,目前已实现，端对端、群聊、广播接口，消息可实时推送，目前仅处理在线消息，离线消息不能发送，由于聊天数据庞大，并发多。实时存储记录会给系统及数据库提供很大的压力，系统采用异步存储的方式，在消息发送时，临时存储到线程安全的缓存中（ConcurrentQueue、Redis），在服务后端开启一个或多个Worker，进行数据的批量处理，增加数据的插入速度，且可根据分库分表等规则，进行不同数据库或表的存储。ChatRoom 地址 http://localhost:5168/chatIndex.html ,需要多开窗口看效果，目前用户是随机模拟，需先启动ChatRoom.ChatServerApi服务，再启动ChatRoom.ChatWeb



代码结构：

  --ChatRoom.Api            			->[webapi接口]：为web端或其他端口提供接口服务
  --ChatRoom.ChatServerApi	->[聊天服务]：为聊天web端提供后台接口服务
  --ChatRoom.ChatWeb        	 ->[聊天模块web]：聊天模块web端
  --ChatRoom.Core          		   ->[基础类库]：提供主要的基础功能、组件或服务
  --ChatRoom.Model          		->[实体层类库]：业务库表、数据传输对象
  --ChatRoom.Repository     	 ->[仓库层类库]：提供数据库操作
  --ChatRoom.Service        	    ->[服务层类库]：处理业务逻辑并提供给Api接口调用



The Demo currently uses the MySQL database. If you need to change the database type, modify the DbType and database link in SugerDBExtensions

script：

```sql
CREATE TABLE tb_message  (`
  `id int(11) NOT NULL AUTO_INCREMENT,`
  `sender varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,`
  `recipient varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,`
  `content varchar(5000) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,`
  `record_time datetime NULL DEFAULT NULL,`
  `PRIMARY KEY (id) USING BTREE`
`) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

CREATE TABLE `tb_user`  (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `user_id` varchar(20) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `user_name` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `password` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `salt` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `nickname` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `phone` varchar(11) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `online_status` bit(1) NOT NULL,
  `create_by` varchar(20) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `create_time` datetime NULL DEFAULT NULL,
  `update_by` varchar(20) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `update_time` datetime NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `idx_user_id`(`user_id`) USING BTREE,
  UNIQUE INDEX `idx_user_name`(`user_name`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;
```

