using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebAppCore31.Models;
using Dapper;
using System.Data;
using System.Data.SqlTypes;
using System.Transactions;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.Extensions.Configuration;
using WebAppCore31.Helper;

namespace WebAppCore31.Repository
{
    public class FileManager : IFileManager
    {
        private IConfiguration Configuration;
        private readonly string connString;
        public FileManager(IConfiguration _configuration)
        {
            Configuration = _configuration;
            connString = this.Configuration.GetConnectionString("DefaultConnection");
        }
        public async Task<Guid> InsFile(FileModel model)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connString))
                {
                    DynamicParameters parameters = new DynamicParameters();

                    parameters.Add("@insertedGuid", model.ContentGuid, direction: ParameterDirection.InputOutput);
                    parameters.Add("@P_FileName", model.FileName);
                    parameters.Add("@P_Extension", model.Extension);
                    parameters.Add("@P_Content", model.Content);
                    parameters.Add("@P_PrivacyType", model.PrivacyType);
                    parameters.Add("@P_CreateDate", model.CreateDate);
                    var a = await connection.ExecuteAsync("Stp_InsertFile", parameters, commandType: System.Data.CommandType.StoredProcedure);

                    return parameters.Get<Guid>("@insertedGuid");
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public async Task<FileModel> GetFile(Guid ContentGuid)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connString))
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@P_ContentGuid", ContentGuid);

                    FileModel model = await connection.QueryFirstOrDefaultAsync<FileModel>("Stp_GetFile", parameters, commandType: System.Data.CommandType.StoredProcedure);
                    return model;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public async Task<FileModel> GetFileByFileId(int FileId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connString))
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@P_FileId", FileId);

                    FileModel model = await connection.QueryFirstOrDefaultAsync<FileModel>("Stp_GetFile", parameters, commandType: System.Data.CommandType.StoredProcedure);
                    return model;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public async Task<FileModel> GetFileByGuid(Guid Guid)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connString))
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@P_ContentGuid", Guid);

                    FileModel model = await connection.QueryFirstOrDefaultAsync<FileModel>("Stp_GetFile", parameters, commandType: System.Data.CommandType.StoredProcedure);
                    return model;

                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public async Task<CustomStream> GetFileStream(Guid id)
        {
            CustomStream stream = new CustomStream();
            SqlConnection sqlConnection = new SqlConnection(connString);
            sqlConnection.Open();
            stream.sqlTransaction = sqlConnection.BeginTransaction();
            string sqlQuery = String.Format("Select Content.PathName() As Path,GET_FILESTREAM_TRANSACTION_CONTEXT() As TransactionContext,Extension,CreateDate,FileName From tbl_Files Where ContentGuid = '{0}'", id);
            using (SqlCommand sqlCommand = new SqlCommand(sqlQuery, sqlConnection, stream.sqlTransaction))
            {
                sqlCommand.Parameters.Add("@primaryKey", SqlDbType.UniqueIdentifier).Value = id;
                stream.sqlDataReader = sqlCommand.ExecuteReader();
            }
            stream.sqlDataReader.Read();

            string filePath = (string)stream.sqlDataReader["Path"];
            byte[] transactionContext = (byte[])stream.sqlDataReader["TransactionContext"];

            DateTime createDate = (DateTime)stream.sqlDataReader["CreateDate"];
            stream.sqlFileStream = new SqlFileStream(filePath, transactionContext, FileAccess.Read);

            return stream;
        }
        public void BeginTransaction(SqlConnection sqlConnection)
        {
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "BEGIN TRANSACTION";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = sqlConnection;

            cmd.ExecuteNonQuery();
        }
        public void CommitTransaction(SqlConnection sqlConnection)
        {
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "COMMIT TRANSACTION";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = sqlConnection;

            cmd.ExecuteNonQuery();
        }
        public Object GetTransactionContext(SqlConnection sqlConnection, Guid id)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = String.Format("Select Content.PathName() As Path,GET_FILESTREAM_TRANSACTION_CONTEXT() As TransactionContext,Extension,CreateDate,FileName From tbl_Files Where ContentGuid = '{0}'", id);
            cmd.CommandType = CommandType.Text;
            cmd.Connection = sqlConnection;

            return cmd.ExecuteScalar();

        }


    }
}
