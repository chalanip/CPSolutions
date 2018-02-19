using CP.DataUtility.SQL;
using System;
using System.Collections.Generic;
using System.Data;

namespace CP.Data
{
    /// <summary>
    /// Handling database data for all transactions related to couple chat services
    /// </summary>
    public class CoupleChatDM
    {
         public CoupleChatDM() { }

        #region Couple Chat App
        public bool CodeExists(string code)
        {
            string commandStr = @"SELECT count(*) FROM couple_users WHERE IsActive = 1 AND Code = @Code;";

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

        public int AddUser(string userNumber, string code)
        {
            int newId = GetUserId(userNumber);
            string commandStr = string.Empty;

            if (newId == 0)
            {
                commandStr = @"
INSERT INTO couple_users (UserNumber, Code, IsActive) 
VALUES (@UserNumber, @Code, 1); 

SET @UserId = LAST_INSERT_ID();   

INSERT INTO couple_usershistory (UserId, Description, DateTimeStamp) 
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

        public bool IsUserLinked(string userNumber)
        {
            int userId = GetUserId(userNumber);
            var status = false;
            if (userId > 0)
            {
                string commandStr = @"
                SELECT count(*) AS RecordCount FROM couple_usercouples WHERE UserId = @UserId OR CoupleUserId = @UserId;";

                var result = Sql.ExecuteScaler(commandStr,
                        new KeyValuePair<string, dynamic>("UserId", userId));

                if (result != null)
                {
                    var count = Int32.Parse(result.ToString());
                    status = (count > 0) ? true : false;
                }
            }
            return status;
        }
        
        public bool LinkUser(string userNumber, string coupleUserAddress)
        {
            int coupleUserId = GetUserId(coupleUserAddress);
            int userId = GetUserId(userNumber);
            
            var status = false;
            if (coupleUserId > 0 && userId > 0)
            {
                var commandStr = @"
                    INSERT INTO couple_usercouples (UserId, CoupleUserId)
                    VALUES(@UserId, @CoupleUserId);
                    ";

                var result = Sql.ExecuteQuery(commandStr,
                        new KeyValuePair<string, dynamic>("UserId", userId),
                        new KeyValuePair<string, dynamic>("CoupleUserId", coupleUserId));

                if (result == 1)
                    status = true;
            }
            
            return status;
        }
                
        public string GetUserNumberByCode(string code)
        {
            string commandStr = @"SELECT UserNumber FROM couple_users WHERE IsActive = 1 AND Code = @Code LIMIT 1;";

            var result = Sql.ExecuteScaler(commandStr,
                    new KeyValuePair<string, dynamic>("Code", code));

            var userNumber = string.Empty;
            if (result != null)
            {
                userNumber = result.ToString();
            }
            
            return userNumber;
        }

        public bool RemoveUser(string userNumber)
        {
            int userId = GetUserId(userNumber);
            bool status = false;
            if (userId > 0)
            {
                string commandStr = @"
UPDATE couple_users SET IsActive = 0 WHERE Id = @UserId;

DELETE FROM couple_usercouples WHERE UserId = @UserId OR CoupleUserId = @UserId;

INSERT INTO couple_usershistory (UserId, Description, DateTimeStamp) 
VALUES (@UserId, 'USER_UNREGISTER', NOW()); 
";

                var result = Sql.ExecuteQuery(commandStr,
                        new KeyValuePair<string, dynamic>("UserId", userId));

                if (result >= 2)
                    status = true;
            }
            
            return status;
        }

        public string GetCoupleUserNumberByUserNumber(string userNumber)
        {            
            string commandStr = @"
SET @UserId = (SELECT Id  FROM couple_users WHERE IsActive = 1 AND UserNumber = @UserNumber LIMIT 1);

SET @CoupleUserId = (
    SELECT DISTINCT UserId FROM(
    SELECT CoupleUserId AS UserId FROM couple_usercouples WHERE UserId = @UserId 
    UNION
    SELECT UserId FROM couple_usercouples WHERE CoupleUserId = @UserId) A LIMIT 1);

SELECT UserNumber FROM couple_users WHERE IsActive = 1 AND Id = @CoupleUserId LIMIT 1;";

            var result = Sql.ExecuteScaler(commandStr,
                    new KeyValuePair<string, dynamic>("UserNumber", userNumber));

            var coupleUserNumber = string.Empty;
            if (result != null)
            {
                coupleUserNumber = result.ToString();
            }
            
            return coupleUserNumber;
        }

        public bool RemoveLinkedUser(string userNumber)
        {
            int userId = GetUserId(userNumber);
            var status = false;
            if (userId > 0)
            {
                var commandStr = @"
                    DELETE FROM couple_usercouples WHERE UserId = @UserId OR CoupleUserId = @UserId;
                    ";

                var result = Sql.ExecuteScaler(commandStr,
                        new KeyValuePair<string, dynamic>("UserId", userId));
                status = true;
            }
            return status;
        }

        public string GetUserCode(string userNumber)
        {
            string commandStr = @"SELECT Code FROM couple_users WHERE IsActive = 1 AND UserNumber = @UserNumber LIMIT 1;";

            var result = Sql.ExecuteScaler(commandStr,
                    new KeyValuePair<string, dynamic>("UserNumber", userNumber));

            var code = string.Empty;
            if (result != null)
            {
                code = result.ToString();
            }

            return code;
        }

        public int GetActiveUserCount()
        {
            string commandStr = @"SELECT IFNULL(Count(*),0) AS ActiveUsers FROM couple_users WHERE IsActive = 1;";

            var result = Sql.ExecuteScaler(commandStr);

            var count = 0;
            if (result != null)
            {
                count = Int32.Parse(result.ToString());
            }
            return count;
        }

        public int GetCoupleUserCount()
        {
            string commandStr = @"SELECT IFNULL(Count(*),0) AS CoupleUsers FROM couple_usercouples;";

            var result = Sql.ExecuteScaler(commandStr);

            var count = 0;
            if (result != null)
            {
                count = Int32.Parse(result.ToString());
            }
            return count;
        }

        #endregion

        #region private methods
        private int GetUserId(string userNumber)
        {
            int newId = 0;
            string commandStr = @"SELECT Id  FROM couple_users WHERE IsActive = 1 AND UserNumber = @UserNumber LIMIT 1;";

            var result = Sql.ExecuteScaler(commandStr,
                       new KeyValuePair<string, dynamic>("UserNumber", userNumber));
            if (result != null)
                newId = Int32.Parse(result.ToString());

            return newId;
        }

        #endregion

        #region not using
        //public List<string> GetAllUserAddressList()
        //{
        //    List<string> userList = new List<string>();

        //    string commandStr = @"SELECT UserNumber FROM couple_users WHERE IsActive = 1;";

        //    DataTable result = Sql.ExecuteDataTable(commandStr);

        //    if (result != null || result.Rows != null || result.Rows.Count > 0)
        //    {
        //        foreach (DataRow dr in result.Rows)
        //        {
        //            userList.Add(dr["UserNumber"].ToString());
        //        }
        //    }

        //    return userList;
        //}

        //public string GetUserCode(string userNumber)
        //{
        //    string commandStr = @"SELECT code  FROM couple_users WHERE IsActive = 1 AND UserNumber = @UserNumber LIMIT 1;";

        //    var result = Sql.ExecuteScaler(commandStr,
        //               new KeyValuePair<string, dynamic>("UserNumber", userNumber));

        //    var code = string.Empty;
        //    if (result != null)
        //        code = result.ToString();
        //    return code;
        //}

        //private int GetUserIdByCode(string code)
        //{
        //    int newId = 0;
        //    string commandStr = @"SELECT Id  FROM couple_users WHERE IsActive = 1 AND Code = @Code LIMIT 1;";

        //    var result = Sql.ExecuteScaler(commandStr,
        //               new KeyValuePair<string, dynamic>("Code", code));
        //    if (result != null)
        //        newId = Int32.Parse(result.ToString());

        //    return newId;
        //}


        /*
SET @CoupleUserId = IFNULL((
    SELECT DISTINCT UserId FROM(
    SELECT CoupleUserId AS UserId FROM couple_usercouples WHERE UserId = @UserId 
    UNION
    SELECT UserId FROM couple_usercouples WHERE CoupleUserId = @UserId) A LIMIT 1),0);

UPDATE couple_users SET IsActive = 0 WHERE Id = @CoupleUserId;

INSERT INTO couple_usershistory (UserId, Description, DateTimeStamp) 
SELECT DISTINCT UserId , 'COUPLE_USER_UNREGISTER', NOW()
FROM(
    SELECT CoupleUserId AS UserId FROM couple_usercouples WHERE UserId = @UserId 
    UNION
    SELECT UserId FROM couple_usercouples WHERE CoupleUserId = @UserId) A LIMIT 1;
    
*/

        #endregion
    }
}
