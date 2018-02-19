using CP.DataUtility.SQL;
using CP.Dto;
using CP.Enum;
using CP.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Transactions;

namespace CP.Data
{
    /// <summary>
    /// Handling database data for all transactions related to services
    /// </summary>
    public class ChatAppDM
    {
        
        /// <summary>
        /// Handling database data for all transactions related to services
        /// </summary>
        public ChatAppDM() { }

        #region Chat App
        public bool CodeExists(string code)
        {
            string commandStr = @"SELECT count(*) FROM users WHERE IsActive = 1 AND Code = @Code;";

            var result = Sql.ExecuteScaler(commandStr,
                    new KeyValuePair<string, dynamic>("Code", code));

            var status = false;
            if (result != null)
            {
                var count = Int32.Parse(result.ToString());
                status = (count > 0) ? true : false;
            }

            return status;
        }

        public int SubscribeUser(string userNumber, string code)
        {
            int newId = GetUserId(userNumber);
            string commandStr = string.Empty;           

            if (newId == 0)
            {
                commandStr = @"
INSERT INTO users (UserNumber, Code, IsActive) 
VALUES (@UserNumber, @Code, 1); 

SET @UserId = LAST_INSERT_ID();   

INSERT INTO subscriptionhistory (UserId, Description, DateTimeStamp) 
VALUES (@UserId, 'USER_REGISTER', NOW()); 

SELECT @UserId;";

                var result = Sql.ExecuteScaler(commandStr,
                        new KeyValuePair<string, dynamic>("UserNumber", userNumber),
                        new KeyValuePair<string, dynamic>("Code", code));

                if (result != null)
                    newId = Int32.Parse(result.ToString());
            }
            
            return newId;
        }

        public bool UnsubscribeUser(string userNumber)
        {
            int userId = GetUserId(userNumber);
            bool status = false;
            if (userId > 0)
            {
                string commandStr = @"
UPDATE users SET IsActive = 0 WHERE Id = @UserId;

INSERT INTO subscriptionhistory (UserId, Description, DateTimeStamp) 
VALUES (@UserId, 'USER_UNREGISTER', NOW()); 
";

                var result = Sql.ExecuteQuery(commandStr,
                        new KeyValuePair<string, dynamic>("UserId", userId));

                if (result == 2)
                    status = true;
            }
            
            return status;
        }

        public string GetUserAddress(string code)
        {
            string coupleUserAddress = string.Empty;

            string commandStr = @"SELECT UserNumber FROM users WHERE Code = @Code AND IsActive = 1 LIMIT 1;";

            var result = Sql.ExecuteScaler(commandStr,
                    new KeyValuePair<string, dynamic>("Code", code));

            if (result != null)
                coupleUserAddress = result.ToString();           
            
            return coupleUserAddress;
        }

        public List<string> GetAllUserAddressList()
        {
            List<string> userList = new List<string>();

            string commandStr = @"SELECT UserNumber FROM users WHERE IsActive = 1;";

            DataTable result = Sql.ExecuteDataTable(commandStr);

            if (result != null || result.Rows != null || result.Rows.Count > 0)
            {
                foreach(DataRow dr in result.Rows)
                {
                    userList.Add(dr["UserNumber"].ToString());
                }
            }
            
            return userList;
        }

        public string GetUserCode(string userNumber)
        {
            string commandStr = @"SELECT code  FROM users WHERE IsActive = 1 AND UserNumber = @UserNumber LIMIT 1;";

            var result = Sql.ExecuteScaler(commandStr,
                       new KeyValuePair<string, dynamic>("UserNumber", userNumber));

            var code = string.Empty;
            if (result != null)
                code = result.ToString();
            return code;
        }

        #endregion

        #region private methods
        private int GetUserId(string userNumber)
        {
            int newId = 0;
            string commandStr = @"SELECT Id  FROM users WHERE IsActive = 1 AND UserNumber = @UserNumber LIMIT 1;";

            var result = Sql.ExecuteScaler(commandStr,
                       new KeyValuePair<string, dynamic>("UserNumber", userNumber));
            if (result != null)
                newId = Int32.Parse(result.ToString());

            return newId;
        }

        #endregion
        
    }
}
