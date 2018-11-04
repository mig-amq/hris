using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    using System.Data;
    using System.Diagnostics;

    using WebApplication1.Models.Supers;

    public enum AccountType
    {
        CEO = 1,
        VP = 2,
        DepartmentHead = 3,
        Employee = 4,
        Applicant = 5,
        Any = 6
    }

    public class Account : SecureObject
    {
        private DBHandler DBHandler;
        public bool Exists { get; set; }
        public AccountType Type { get; set; }
        public int AccountID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public bool Locked { get; set; }
        public string Security { get; set; }
        public string Answer { get; set; }
        public ProfiledObject Profile { get; set; }

        public Account()
        {
            this.DBHandler = new DBHandler();
        }

        public Account FindByUsername(string username, bool recursive = true)
        {
            using (DataTable result = this.DBHandler.Execute<DataTable>(CRUD.READ, "SELECT * FROM Account WHERE Username = '" + Sanitize(username) + "'"))
            {
                if (result.Rows.Count > 0)
                {
                    DataRow row = result.Rows[0];
                    this.Username = row["Username"].ToString();
                    this.Password = row["Password"].ToString();
                    this.Email = row["Email"].ToString();
                    this.AccountID = Int32.Parse(row["AccountID"].ToString());
                    this.Type = (AccountType)Int32.Parse(row["Type"].ToString());
                    this.Locked = Int32.Parse(row["Locked"].ToString()) == 1;
                    this.Security = row["Security"].ToString();
                    this.Answer = row["Answer"].ToString();

                    if (recursive)
                    {
                        if (this.Type == AccountType.Applicant)
                        {
                            this.Profile = new Applicant(Int32.Parse(row["Profile"].ToString()));
                        }
                        else
                        {
                            this.Profile = new Employee(Int32.Parse(row["Profile"].ToString()));
                        }
                    }
                    this.Exists = true;
                }
                else
                {
                    this.Exists = false;
                }
            }

            return this;
        }

        public Account FindById(int id, bool recursive = true)
        {
            using (DataTable result = this.DBHandler.Execute<DataTable>(CRUD.READ, "SELECT * FROM Account WHERE AccountID = " + id))
            {
                if (result.Rows.Count > 0)
                {
                    DataRow row = result.Rows[0];
                    this.Username = row["Username"].ToString();
                    this.Password = row["Password"].ToString();
                    this.Email = row["Email"].ToString();
                    this.AccountID = Int32.Parse(row["AccountID"].ToString());
                    this.Type = (AccountType) Enum.Parse(typeof(AccountType), row["Type"].ToString(), true);
                    Debug.WriteLine("Locked: ", row["Locked"].ToString());
                    this.Locked = Int32.Parse(row["Locked"].ToString()) == 1;
                    this.Security = row["Security"].ToString();
                    this.Answer = row["Answer"].ToString();

                    if (recursive)
                    {
                        if (this.Type == AccountType.Applicant)
                        {
                            this.Profile = new Applicant(Int32.Parse(row["Profile"].ToString()));
                        }
                        else
                        {
                            this.Profile = new Employee(Int32.Parse(row["Profile"].ToString()));
                        }
                    }
                    this.Exists = true;
                }
                else
                {
                    this.Exists = false;
                }
            }
            return this;
        }

        public Account FindByProfile(int ProfileID, bool recursive = true)
        {
            using (DataTable dt = this.DBHandler.Execute<DataTable>(
                CRUD.READ,
                "SELECT AccountID FROM Account WHERE Profile = " + ProfileID))
            {
                if (dt.Rows.Count > 0)
                {
                    this.FindById(Int32.Parse(dt.Rows[0]["AccountID"].ToString()), recursive);
                }
            }
            return this;
        }

        public Account Create(int DepartmentID = -1, bool recursive = true)
        {
            string columns = "INSERT INTO Account(Username, Email, Password, Profile, Type, Security, Answer) OUTPUT INSERTED.AccountID ";
            string values = "VALUES(@Username, @Email, @Password, @Profile, @Type, @Security, @Answer)";
            Dictionary<string, dynamic> param = new Dictionary<string, dynamic>();


            if (recursive)
            {
                if (this.Type == AccountType.Applicant)
                {
                    ((Applicant)this.Profile).Create();
                }
                else if (DepartmentID != -1)
                {
                    ((Employee)this.Profile).Create(DepartmentID, recursive);
                }
                else
                {
                    throw new Exception("Failed to create a profile for this Account.");
                }
            }

            param.Add("@Username", this.Username);
            param.Add("@Email", this.Email);
            param.Add("@Password", this.Password);
            param.Add("@Profile", this.Profile.Profile.ProfileID);
            param.Add("@Type", (int)this.Type);
            param.Add("@Security", this.Security);
            param.Add("@Answer", this.Answer);

            this.DBHandler.Execute<Int32>(CRUD.CREATE, columns + values, param);
            return this;

        }

        public Account Update(bool recursive = true)
        {
            return this.Update(this.AccountID, recursive);
        }

        public Account Update(int AccountID, bool recursive)
        {
            if (recursive)
            {
                if (this.Profile != null)
                {
                    if (this.Type == AccountType.Applicant)
                    {
                        ((Applicant)this.Profile).Update(recursive);
                    }
                    else
                    {
                        ((Employee)this.Profile).Update(recursive);
                    }
                }
            }
            string set = "UPDATE Account SET Username = @Username, Password = @Password, "
                         + "Email = @Email, Locked = @Locked, Type = @Type, Security = @Security, Answer = @Answer"
                         + " OUTPUT INSERTED.AccountID WHERE AccountID = " + AccountID;
            Dictionary<string, dynamic> param = new Dictionary<string, dynamic>();

            param.Add("@Username", this.Username);
            param.Add("@Password", this.Password);
            param.Add("@Email", this.Email);
            param.Add("@Locked", this.Locked ? 1 : 0);
            if (this.Profile != null)
                param.Add("@Profile", this.Profile.Profile.ProfileID);

            param.Add("@Type", (int)this.Type);
            param.Add("@Security", this.Security);
            param.Add("@Answer", this.Answer);

            this.DBHandler.Execute<Int32>(CRUD.UPDATE, set, param);
            return this;
        }

        public Account Delete()
        {
            this.DBHandler.Execute<Int32>(CRUD.DELETE, "DELETE FROM Account WHERE AccountID = " + this.AccountID);
            if (this.Type == AccountType.Applicant)
            {
                ((Applicant)this.Profile).Delete();
            }
            else
            {
                ((Employee)this.Profile).Delete();
            }
            return this;
        }
    }
}