using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    using System.Data;

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

        public Account FindById(int id, bool recursive = false)
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
                    this.Locked = Boolean.Parse(row["Locked"].ToString());

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

        public Account Find(string username, string password, bool hashed)
        {
            return this;
        }

        public Account Find(string username, string password)
        {
            return this;
        }

        public Account Find(string username)
        {
            return this;
        }

        public Account Create(int DepartmentID = -1)
        {
            string columns = "INSERT INTO Account(Username, Email, Password, Profile, Type) OUTPUT INSERTED.AccountID ";
            string values = "VALUES(@Username, @Email, @Password, @Profile, @Type)";
            Dictionary<string, dynamic> param = new Dictionary<string, dynamic>();


            if (this.Type == AccountType.Applicant)
            {
                ((Applicant)this.Profile).Create();
            }
            else if (DepartmentID != -1)
            {
                ((Employee)this.Profile).Create(DepartmentID);
            }
            else
            {
                throw new Exception("Failed to create a profile for this Account.");
            }

            param.Add("@Username", this.Username);
            param.Add("@Email", this.Email);
            param.Add("@Password", this.Password);
            param.Add("@Profile", this.Profile.Profile.ProfileID);
            param.Add("@Type", (int)this.Type);

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
            string set = "UPDATE Account SET Username = @Username AND Password = @Password AND "
                         + "Email = @Email AND Locked = @Locked AND Type = @Type WHERE AccountID = " + AccountID;
            Dictionary<string, dynamic> param = new Dictionary<string, dynamic>();

            param.Add("@Username", this.Username);
            param.Add("@Password", this.Password);
            param.Add("@Email", this.Email);
            param.Add("@Locked", this.Locked ? 1 : 0);
            param.Add("@Type", (int)this.Type);

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