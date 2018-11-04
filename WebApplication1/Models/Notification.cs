using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public enum NotificationStatus
    {
        Unread = 1,
        Read = 2
    }

    public class Notification
    {
        private DBHandler DBHandler;

        public int NotificationID { get; set; }
        public Account Account { get; set; }
        public string Message { get; set; }
        public NotificationStatus Status { get; set; }
        public DateTime TimeStamp { get; set; }

        public Notification(int TableID = -1, bool recursive = true, bool byPrimary = true)
        {
            this.DBHandler = new DBHandler();

            if (TableID != -1)
            {
                if (byPrimary) this.NotificationID = TableID;
                this.Find(TableID, recursive, byPrimary);
            }
        }
        public Notification Find(int TableID, bool recursive = true, bool byPrimary = true)
        {
            using (DataTable dt = this.DBHandler.Execute<DataTable>(CRUD.READ,
                "SELECT * FROM Notification WHERE " + (byPrimary ? " NotificationID " : " Account ") + " = " + TableID))
            {
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];

                    this.NotificationID = Int32.Parse(row["NotificationID"].ToString());
                    this.Message = row["Message"].ToString();
                    this.TimeStamp = DateTime.Parse(row["TimeStamp"].ToString());
                    this.Status = (NotificationStatus)Int32.Parse(row["Status"].ToString());

                    if (recursive)
                        this.Account = new Account().FindById(Int32.Parse(row["Account"].ToString()), true);
                }
            }
            return this;
        }

        public Notification Create()
        {
            string sql = "INSERT INTO Notification(Account, Message, Timestamp, Status) "
                         + "OUTPUT INSERTED.NotificationID VALUES(@Account, @Message, @Timestamp, @Status)";
            Dictionary<string, dynamic> param = new Dictionary<string, dynamic>();

            param.Add("@Account", this.Account.AccountID);
            param.Add("@Message", this.Message);
            param.Add("@Timestamp", this.TimeStamp.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            param.Add("@Status", (int)this.Status);

            this.NotificationID = this.DBHandler.Execute<Int32>(CRUD.CREATE, sql, param);
            return this;
        }

        public Notification Update(bool recursive)
        {
            return this.Update(this.NotificationID, recursive);
        }

        public Notification Update(int TableID, bool recursive, bool byPrimary = true)
        {
            string sql = "UPDATE Notification SET Account = @Account, Message = @Message,"
                         + " Timestamp = @Timestamp, Status = @Status OUTPUT INSERTED.NotificationID "
                         + "WHERE NotificationID = " + this.NotificationID;
            Dictionary<string, dynamic> param = new Dictionary<string, dynamic>();

            param.Add("@Account", this.Account.AccountID);
            param.Add("@Message", this.Message);
            param.Add("@Timestamp", this.TimeStamp.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            param.Add("@Status", (int)this.Status);

            this.DBHandler.Execute<Int32>(CRUD.UPDATE, sql, param);
            return this;
        }


    }
}