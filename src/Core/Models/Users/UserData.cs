﻿using System;
using System.Data;
using PlatoCore.Abstractions.Extensions;
using PlatoCore.Abstractions;

namespace PlatoCore.Models.Users
{
    
    public class UserData : IDbModel<UserData>
    {

        #region "Public Properties"

        public int Id { get; set; }

        public int UserId { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }

        public DateTimeOffset? CreatedDate { get; set; }

        public int CreatedUserId { get; set; }

        public DateTimeOffset? ModifiedDate { get; set; }

        public int ModifiedUserId { get; set; }

        #endregion

        #region "Constructor"

        public UserData()
        {
        }

        public UserData(IDataReader reader)
        {
            PopulateModel(reader);
        }

        #endregion

        #region "Implementation"

        public void PopulateModel(IDataReader dr)
        {

            if (dr.ColumnIsNotNull("Id"))
            {
                Id = Convert.ToInt32(dr["Id"]);
            }


            if (dr.ColumnIsNotNull("UserId"))
            {
                UserId = Convert.ToInt32(dr["UserId"]);
            }

            if (dr.ColumnIsNotNull("Key"))
            {
                Key = Convert.ToString(dr["Key"]);
            }

            if (dr.ColumnIsNotNull("Value"))
            {
                Value = Convert.ToString(dr["Value"]);
            }

            if (dr.ColumnIsNotNull("CreatedDate"))
            {
                CreatedDate = (DateTimeOffset)dr["CreatedDate"];
            }

            if (dr.ColumnIsNotNull("CreatedUserId"))
            {
                CreatedUserId = Convert.ToInt32(dr["CreatedUserId"]);
            }

            if (dr.ColumnIsNotNull("ModifiedUserId"))
            {
                ModifiedUserId = Convert.ToInt32((dr["ModifiedUserId"]));
            }

            if (dr.ColumnIsNotNull("ModifiedDate"))
            {
                ModifiedDate = (DateTimeOffset)dr["ModifiedDate"];
            }

        }

        #endregion
        
    }

}
