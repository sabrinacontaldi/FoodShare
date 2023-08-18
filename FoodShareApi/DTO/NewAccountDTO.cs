using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodShareApi.DTO
{
    public class NewAccountDTO
    {
        //NEW FEEDER/DONOR
        public string organization { get; set; }
        public string description { get; set; }
        public string branch { get; set; }
        public int zipCode { get; set; }

       
        public string username { get; set; }
        public string contact_person { get; set; }
        public string email_address { get; set; }
        public int? feeder_id { get; set; } = null;
        public int? donor_id { get; set; } = null;

        public string password { get; set; }
        //  newF.Organization = model.Organization;
        //         newF.Description = model.Description;
        //         newF.Branch = model.Branch;
        //         newF.ZipCode = model.ZipCode;

        //         //new user
        //         //id of new feeder is set to the user's feeder_id
        //         newUser.feeder_id = await FeederService.Register(newF);

        //         newUser.contact_person = model.Name;
        //         newUser.email_address = model.EmailAddress;
        //         newUser.username = model.Username;

        //         //new password
        //         //id of new user is set to the password's id
        //         newPassword.Id = await UserService.Register(newUser);

        //         newPassword.Password = model.Password;
                
        //         await PasswordService.Register(newPassword);
                
        //         loading = false;
                
        //         NavigationManager.NavigateTo("/account/login");
    }
}