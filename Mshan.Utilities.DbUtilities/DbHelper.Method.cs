﻿//-----------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2015 , Mshan, Ltd. 
//-----------------------------------------------------------------

using System;
using System.Data;
using System.Data.Common;


namespace Mshan.Utilities.DbUtilities
{
    /// <summary>
    /// DbHelper
    /// 有关数据库连接的方法。
    /// 
    /// 修改纪录
    /// 
    ///		2013.02.03 版本：1.0 Mshan 创建,分离方法。
    /// 
    /// <author>
    ///		<name>Mshan</name>
    ///		<date>2013.02.03</date>
    /// </author> 
    /// </summary>
    public partial class DbHelper
    {
        /// <summary>
        /// 获得数据库当前日期
        /// </summary>
        /// <returns>当前日期</returns>
        public static string GetDbNow(CurrentDbType dbType)
        {
            string result = string.Empty;
            if (dbType == CurrentDbType.SqlServer)
            {
                result = " Getdate() ";
            }
            else if (dbType == CurrentDbType.Oracle)
            {
                result = " SYSDATE ";
            }
            else if (dbType == CurrentDbType.SQLite)
            {
                result = " datetime(CURRENT_TIMESTAMP, 'localtime') ";
            }
            return result;
        }

        public static IDbDataParameter MakeParameter(string targetFiled, object targetValue)
        {
            IDbHelper dbHelper = DbHelperFactory.GetHelper(DbType);
            return dbHelper.MakeParameter(targetFiled, targetValue);
        }

        #region public static string GetParameter(string parameter) 获得参数Sql表达式
        /// <summary>
        /// 获得参数Sql表达式
        /// </summary>
        /// <param name="parameter">参数名称</param>
        /// <returns>字符串</returns>
        public static string GetParameter(string parameter)
        {
            return GetParameter(DbHelper.DbType, parameter);
        }
        #endregion

        #region public static string GetParameter(string parameter) 获得参数Sql表达式
        /// <summary>
        /// 获得参数Sql表达式
        /// </summary>
        /// <param name="parameter">参数名称</param>
        /// <returns>字符串</returns>
        public static string GetParameter(CurrentDbType currentDbType, string parameter)
        {
            switch (currentDbType)
            {
                case CurrentDbType.SqlServer:
                case CurrentDbType.Access:
                case CurrentDbType.Ase:
                case CurrentDbType.PostgreSql:
                case CurrentDbType.SQLite:
                    parameter = "@" + parameter;
                    break;
                case CurrentDbType.DB2:
                case CurrentDbType.Oracle:
                    parameter = ":" + parameter;
                    break;
                case CurrentDbType.MySql:
                    parameter = "?" + parameter;
                    break;
            }
            return parameter;
        }
        #endregion

        #region public static int ExecuteNonQuery(string commandText, IDbDataParameter[] dbParameters = null, CommandType commandType = CommandType.Text) 执行查询返回受影响的行数
        /// <summary>
        /// 执行查询返回受影响的行数
        /// </summary>
        /// <param name="commandText">sql查询</param>
        /// <param name="dbParameters">参数集</param>
        /// <param name="commandType">命令分类</param>
        /// <returns>影响行数</returns>
        public static int ExecuteNonQuery(string commandText, IDbDataParameter[] dbParameters = null, CommandType commandType = CommandType.Text)
        {
            return ExecuteNonQuery(DbConnection, commandText, dbParameters, commandType);
        }
        #endregion

        #region public static int ExecuteNonQuery(string connectionString, string commandText, IDbDataParameter[] dbParameters = null, CommandType commandType = CommandType.Text)
        /// <summary>
        /// 符合多数据库连接的查询方式
        /// </summary>
        /// <param name="connectionString">数据库连接串</param>
        /// <param name="commandText">sql查询</param>
        /// <param name="dbParameters">参数集</param>
        /// <param name="commandType">命令分类</param>
        /// <returns>影响行数</returns>
        public static int ExecuteNonQuery(string connectionString, string commandText, IDbDataParameter[] dbParameters = null, CommandType commandType = CommandType.Text)
        {
            int result = 0;
            IDbHelper dbHelper = DbHelperFactory.GetHelper(DbType, connectionString);
            result = dbHelper.ExecuteNonQuery(commandText, dbParameters, commandType);
            dbHelper.Close();
            return result;
        }
        #endregion

        #region public static object ExecuteScalar(string commandText, IDbDataParameter[] dbParameters = null, CommandType commandType = CommandType.Text) 执行查询返回受首行首列
        /// <summary>
        /// 执行查询返回受首行首列
        /// </summary>
        /// <param name="commandText">sql查询</param>
        /// <param name="dbParameters">参数集</param>
        /// <param name="commandType">命令分类</param>
        /// <returns>object</returns>
        public static object ExecuteScalar(string commandText, IDbDataParameter[] dbParameters = null, CommandType commandType = CommandType.Text)
        {
            return ExecuteScalar(DbConnection, commandText, dbParameters, commandType);
        }
        #endregion

        #region public static object ExecuteScalar(string connectionString, string commandText, IDbDataParameter[] dbParameters = null, CommandType commandType = CommandType.Text) 执行查询返回受首行首列
        /// <summary>
        /// 执行查询返回受首行首列
        /// </summary>
        /// <param name="connectionString">数据库连接</param>
        /// <param name="commandText">sql查询</param>
        /// <param name="dbParameters">参数集</param>
        /// <param name="commandType">命令分类</param>
        /// <returns>object</returns>
        public static object ExecuteScalar(string connectionString, string commandText, IDbDataParameter[] dbParameters = null, CommandType commandType = CommandType.Text)
        {
            object result = null;
            IDbHelper dbHelper = DbHelperFactory.GetHelper(DbType, connectionString);
            result = dbHelper.ExecuteScalar(commandText, dbParameters, commandType);
            dbHelper.Close();
            return result;
        }
        #endregion

        #region public static IDataReader ExecuteReader(string commandText, IDbDataParameter[] dbParameters = null, CommandType commandType = CommandType.Text) 执行查询返回DataReader
        /// <summary>
        /// 执行查询返回DataReader
        /// </summary>
        /// <param name="commandType">命令分类</param>
        /// <param name="commandText">sql查询</param>
        /// <param name="dbParameters">参数集</param>
        /// <returns>结果集流</returns>
        public static IDataReader ExecuteReader(string commandText, IDbDataParameter[] dbParameters = null, CommandType commandType = CommandType.Text)
        {
            return ExecuteReader(DbConnection, commandText, dbParameters, commandType);
        }
        #endregion

        #region public static IDataReader ExecuteReader(string connectionString, string commandText, IDbDataParameter[] dbParameters = null, CommandType commandType = CommandType.Text) 执行查询返回DataReader
        /// <summary>
        /// 执行查询返回DataReader
        /// </summary>
        /// <param name="connectionString">数据库连接</param>
        /// <param name="commandType">命令分类</param>
        /// <param name="commandText">sql查询</param>
        /// <param name="dbParameters">参数集</param>
        /// <returns>结果集流</returns>
        public static IDataReader ExecuteReader(string connectionString, string commandText, IDbDataParameter[] dbParameters = null, CommandType commandType = CommandType.Text)
        {
            IDbHelper dbHelper = DbHelperFactory.GetHelper(DbType, connectionString);
            dbHelper.MustCloseConnection = true;
            return dbHelper.ExecuteReader(commandText, dbParameters, commandType);
        }
        #endregion

        #region public static DataTable Fill(string commandText, IDbDataParameter[] dbParameters, CommandType commandType = CommandType.Text) 填充数据表
        /// <summary>
        /// 填充数据表
        /// </summary>
        /// <param name="commandText">sql查询</param>
        /// <param name="dbParameters">参数集</param>
        /// <param name="commandType">命令分类</param>
        /// <returns>数据表</returns>
        public static DataTable Fill(string commandText, IDbDataParameter[] dbParameters = null, CommandType commandType = CommandType.Text)
        {
            return Fill(DbConnection, commandText, dbParameters, commandType);
        }
        #endregion

        #region public static DataTable Fill(string connectionString, string commandText, IDbDataParameter[] dbParameters, CommandType commandType = CommandType.Text) 填充数据表
        /// <summary>
        /// 填充数据表
        /// </summary>
        /// <param name="connectionString">数据库连接</param>
        /// <param name="commandText">sql查询</param>
        /// <param name="dbParameters">参数集</param>
        /// <param name="commandType">命令分类</param>
        /// <returns>数据表</returns>
        public static DataTable Fill(string connectionString, string commandText, IDbDataParameter[] dbParameters = null, CommandType commandType = CommandType.Text)
        {
            var dt = new DataTable("DotNet");
            IDbHelper dbHelper = DbHelperFactory.GetHelper(DbType, connectionString);
            dbHelper.Fill(dt, commandText, dbParameters, commandType);
            dbHelper.Close();
            return dt;
        }
        #endregion

        #region public static void ExecuteCommandWithSplitter(string commandText, string splitter) 批量执行脚本的方法
        /// <summary>
        /// 运行含有GO命令的多条SQL命令
        /// </summary>
        /// <param name="commandText">SQL命令字符串</param>
        /// <param name="splitter">分割字符串</param>
        public static void ExecuteCommandWithSplitter(string commandText, string splitter)
        {
            int startPos = 0;
            do
            {
                int lastPos = commandText.IndexOf(splitter, startPos);
                int length = (lastPos > startPos ? lastPos : commandText.Length) - startPos;
                string query = commandText.Substring(startPos, length);

                if (query.Trim().Length > 0)
                {
                    ExecuteNonQuery(query, null, CommandType.Text);
                }

                if (lastPos == -1)
                {
                    break;
                }
                else
                {
                    startPos = lastPos + splitter.Length;
                }
            } while (startPos < commandText.Length);
        }

        /// <summary>
        /// 运行含有GO命令的多条SQL命令
        /// </summary>
        /// <param name="commandText">SQL命令字符串</param>
        public static void ExecuteCommandWithSplitter(string commandText)
        {
            ExecuteCommandWithSplitter(commandText, "\r\nGO\r\n");
        }
        #endregion ExecuteCommandWithSplitter方法结束
    }
}
