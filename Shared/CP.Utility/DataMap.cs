using CP.Constants;
using CP.Dto;
using CP.Enum;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP.Utility
{
    public static class DataMap
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static List<UserDto> MapUserData(DataTable dataTable)
        {
            Log.TraceStart();
            List<UserDto> userList = new List<UserDto>();

            foreach (DataRow dr in dataTable.Rows)
            {
                UserDto user = new UserDto();
                user.Id = Int32.Parse(dr[Consts.COL_User_ID].ToString());
                user.Name = dr[Consts.COL_User_Name].ToString();
                user.NIC = dr[Consts.COL_User_NIC].ToString();
                user.Town = dr[Consts.COL_User_Town].ToString();
                user.ContactNumber = dr[Consts.COL_User_ContactNo].ToString();
                user.Category = EnumHandler.ParseToEnum<Category>(dr[Consts.COL_Category_Name].ToString());

                userList.Add(user);
            }

            Log.TraceEnd();
            return userList;
        }


    }
}
