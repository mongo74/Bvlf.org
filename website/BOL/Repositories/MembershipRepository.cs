using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using System.Xml;
using bvlf_v2.BOL.Data;
using bvlf_v2.Helpers;
using bvlf_v2.Models;
using umbraco.BusinessLogic;
using umbraco.cms.businesslogic.member;

namespace bvlf_v2.BOL.Repositories
{
    public class MembershipRepository
    {
        private readonly bvlf_org_v2Entities _context;

        public MembershipRepository()
        {
            _context = new bvlf_org_v2Entities();
        }

        public Member GetMemberById(int id)
        {
            var member = new Member(id);
            return member;
        }

        public Member GetMemberByEmail(string email)
        {
            var member = Member.GetMemberFromEmail(email);
            return member ?? null;
        }

        public bool MemberIsAllowed(string login)
        {
            // var member = Member.GetMemberFromEmail(login);
            var member = Member.GetMemberFromLoginName(login);
            if (member == null) return false;
            var propValue = member.getProperty("bvlfMemberStatus").Value.ToString();
            var intValue = !string.IsNullOrEmpty(propValue) ? Convert.ToInt32(propValue) : 0;
            if (intValue == (int) BvlfMemberStatus.Slapend || intValue == (int) BvlfMemberStatus.Verwijderd ||
                intValue == 0)
            {
                return false;
            }
            return true;
        }

        public string CreateNewMember(MemberProfileViewModel model)
        {
            try
            {
                var mt = MemberType.GetByAlias("Lid");
                var user = new User(0);

                var member = Member.MakeNew(model.Email, mt, user);

                member.Email = model.Email;
                member.Password = model.Passwoord;
                member.LoginName = model.Email;
                var lidnr = GenerateLidnr();
                //// custom properties

                // NAW gegevens
                member.getProperty("naam").Value = model.LastName;
                member.getProperty("voornaam").Value = model.FirstName;
                member.getProperty("straat").Value = model.Street;
                member.getProperty("nr").Value = model.Nr;
                member.getProperty("bus").Value = model.Box;
                member.getProperty("postcode").Value = model.Zip;
                member.getProperty("plaats").Value = model.Location;
                member.getProperty("tel").Value = model.Tel;
                member.getProperty("mobile").Value = model.Mobile;
                member.getProperty("country").Value = model.Country;
                member.getProperty("subscriptionDate").Value = DateTime.Now;
                member.getProperty("subscriptionExpiration").Value = DateTime.Now.AddYears(1);
                member.getProperty("gender").Value =
                    ((Gender) Convert.ToInt32(model.rbGenderOptions.SelectedValue)).ToString();

                member.getProperty("fipfnr").Value = model.FipfNr; // TODO Generate number
                member.getProperty("lidnr").Value = lidnr;

                member.getProperty("bvlfMemberStatus").Value = (int) BvlfMemberStatus.Ingeschreven;
                member.getProperty("profiel").Value = ((BvlfMemberProfile) model.MemberProfile).ToString();

                // school 1
                member.getProperty("naamschool_1").Value = model.School1.SchoolNaam;
                member.getProperty("straatschool_1").Value = model.School1.Street;
                member.getProperty("nrSchool_1").Value = model.School1.Nr;
                member.getProperty("busschool_1").Value = model.School1.Box;
                member.getProperty("zipcodeschool_1").Value = model.School1.Zip;
                member.getProperty("plaatsschool_1").Value = model.School1.Location;
                member.getProperty("telschool_1").Value = model.School1.Tel;
                member.getProperty("emailschool_1").Value = model.School1.Email;

                // school 2
                member.getProperty("naamschool_2").Value = model.School2.SchoolNaam;
                member.getProperty("straatschool_2").Value = model.School2.Street;
                member.getProperty("nrschool_2").Value = model.School2.Nr;
                member.getProperty("busschool_2").Value = model.School2.Box;
                member.getProperty("zipcodeschool_2").Value = model.School2.Zip;
                member.getProperty("plaatsschool_2").Value = model.School2.Location;
                member.getProperty("telschool_2").Value = model.School2.Tel;
                member.getProperty("emailschool_2").Value = model.School2.Email;

                var group = MemberGroup.GetByName("Abonnee");
                member.AddGroup(group.Id);

                member.Save();

                member.XmlGenerate(new XmlDocument());
                Member.AddMemberToCache(member);

                return lidnr;
            }
            catch (Exception ex)
            {
                // do something with the error
                return ex.Message;
            }
        }

        public int CreateNewMemberForStudiedagOnly(MemberProfileViewModel model)
        {
            try
            {
                // see if member does not exist!!
                var existingMember = Member.GetMemberFromEmail(model.Email);
                //   var member = null;
                var id = 0;

                if (existingMember != null)
                {
                    //    SaveMemberData(model, existingMember);
                    UpdateMemberData(model, existingMember);
                    id = existingMember.Id;
                }
                else
                {
                    var mt = MemberType.GetByAlias("Lid");
                    var user = new User(0);
                    var member = Member.MakeNew(model.Email, mt, user);
                    SaveMemberData(model, member);
                    id = member.Id;
                }

                return id;
            }
            catch (Exception ex)
            {
                //return ex.;
                return -1;
            }
        }

        private void UpdateMemberData(MemberProfileViewModel model, Member member)
        {
            // NAW gegevens
            member.getProperty("naam").Value = model.LastName;
            member.getProperty("voornaam").Value = model.FirstName;
            member.getProperty("straat").Value = model.Street;
            member.getProperty("nr").Value = model.Nr;
            member.getProperty("bus").Value = model.Box;
            member.getProperty("postcode").Value = model.Zip;
            member.getProperty("plaats").Value = model.Location;
            member.getProperty("tel").Value = model.Tel;
            member.getProperty("mobile").Value = model.Mobile;
            member.getProperty("country").Value = model.Country;
            //  member.getProperty("subscriptionDate").Value = DateTime.Now;
            //   member.getProperty("subscriptionExpiration").Value = DateTime.Now.AddYears(1);
            member.getProperty("gender").Value =
                ((Gender) Convert.ToInt32(model.rbGenderOptions.SelectedValue)).ToString();

            member.getProperty("fipfnr").Value = model.FipfNr; // TODO Generate number

            member.getProperty("bvlfMemberStatus").Value = (int) BvlfMemberStatus.Studiedag;
            member.getProperty("profiel").Value = ((BvlfMemberProfile) model.MemberProfile).ToString();

            // school 1
            member.getProperty("naamschool_1").Value = model.School1.SchoolNaam;
            member.getProperty("straatschool_1").Value = model.School1.Street;
            member.getProperty("nrSchool_1").Value = model.School1.Nr;
            member.getProperty("busschool_1").Value = model.School1.Box;
            member.getProperty("zipcodeschool_1").Value = model.School1.Zip;
            member.getProperty("plaatsschool_1").Value = model.School1.Location;
            member.getProperty("telschool_1").Value = model.School1.Tel;
            member.getProperty("emailschool_1").Value = model.School1.Email;

            // school 2
            member.getProperty("naamschool_2").Value = model.School2.SchoolNaam;
            member.getProperty("straatschool_2").Value = model.School2.Street;
            member.getProperty("nrschool_2").Value = model.School2.Nr;
            member.getProperty("busschool_2").Value = model.School2.Box;
            member.getProperty("zipcodeschool_2").Value = model.School2.Zip;
            member.getProperty("plaatsschool_2").Value = model.School2.Location;
            member.getProperty("telschool_2").Value = model.School2.Tel;
            member.getProperty("emailschool_2").Value = model.School2.Email;

            var group = MemberGroup.GetByName("Abonnee");
            member.AddGroup(@group.Id);

            member.Save();

            member.XmlGenerate(new XmlDocument());
            //       Member.AddMemberToCache(member);
        }

        private void SaveMemberData(MemberProfileViewModel model, Member member)
        {
            member.Email = model.Email;
            member.Password = model.Passwoord ?? "Passw00rd";
            member.LoginName = model.Email;
            var lidnr = GenerateLidnr();
            //// custom properties

            // NAW gegevens
            member.getProperty("naam").Value = model.LastName;
            member.getProperty("voornaam").Value = model.FirstName;
            member.getProperty("straat").Value = model.Street;
            member.getProperty("nr").Value = model.Nr;
            member.getProperty("bus").Value = model.Box;
            member.getProperty("postcode").Value = model.Zip;
            member.getProperty("plaats").Value = model.Location;
            member.getProperty("tel").Value = model.Tel;
            member.getProperty("mobile").Value = model.Mobile;
            member.getProperty("country").Value = model.Country;
            member.getProperty("subscriptionDate").Value = DateTime.Now;
            member.getProperty("subscriptionExpiration").Value = DateTime.Now.AddYears(1);
            member.getProperty("gender").Value =
                ((Gender) Convert.ToInt32(model.rbGenderOptions.SelectedValue)).ToString();

            member.getProperty("fipfnr").Value = model.FipfNr; // TODO Generate number
            member.getProperty("lidnr").Value = lidnr;

            member.getProperty("bvlfMemberStatus").Value = (int) BvlfMemberStatus.Studiedag;
            member.getProperty("profiel").Value = ((BvlfMemberProfile) model.MemberProfile).ToString();

            // school 1
            member.getProperty("naamschool_1").Value = model.School1.SchoolNaam;
            member.getProperty("straatschool_1").Value = model.School1.Street;
            member.getProperty("nrSchool_1").Value = model.School1.Nr;
            member.getProperty("busschool_1").Value = model.School1.Box;
            member.getProperty("zipcodeschool_1").Value = model.School1.Zip;
            member.getProperty("plaatsschool_1").Value = model.School1.Location;
            member.getProperty("telschool_1").Value = model.School1.Tel;
            member.getProperty("emailschool_1").Value = model.School1.Email;

            // school 2
            member.getProperty("naamschool_2").Value = model.School2.SchoolNaam;
            member.getProperty("straatschool_2").Value = model.School2.Street;
            member.getProperty("nrschool_2").Value = model.School2.Nr;
            member.getProperty("busschool_2").Value = model.School2.Box;
            member.getProperty("zipcodeschool_2").Value = model.School2.Zip;
            member.getProperty("plaatsschool_2").Value = model.School2.Location;
            member.getProperty("telschool_2").Value = model.School2.Tel;
            member.getProperty("emailschool_2").Value = model.School2.Email;

            var group = MemberGroup.GetByName("Abonnee");
            member.AddGroup(@group.Id);

            member.Save();

            member.XmlGenerate(new XmlDocument());
            //    Member.AddMemberToCache(member);
        }

        public string GenerateLidnr()
        {
            var lidnr = GenerateCode();
            var allMembers = GetAllMembers();
            return lidnr;
        }

        private string GenerateCode()
        {
            var guid = Guid.NewGuid();
            var code = guid.ToString().Substring(0, 4);
            var year = DateTime.Now.ToString("yy");
            return string.Format("{0}-{1}", year, code.ToUpper());
        }

        public string UpdateMemberByAdmin(MemberProfileForAdminUpdateViewModel model)
        {
            try
            {
                var member = new Member(model.Id);
                member.getProperty("naam").Value = model.LastName;
                member.getProperty("voornaam").Value = model.FirstName;
                member.getProperty("straat").Value = model.Street;
                member.getProperty("nr").Value = model.Nr;
                member.getProperty("bus").Value = model.Box;
                member.getProperty("postcode").Value = model.Zip;
                member.getProperty("plaats").Value = model.Location;
                member.getProperty("tel").Value = model.Tel;
                member.getProperty("mobile").Value = model.Mobile;
                member.getProperty("country").Value = model.Country;

                member.getProperty("gender").Value =
                    ((Gender) Convert.ToInt32(model.rbGenderOptions.SelectedValue)).ToString();
                member.getProperty("profiel").Value = ((BvlfMemberProfile) model.MemberProfile).ToString();

                // school 1
                member.getProperty("naamschool_1").Value = model.School1.SchoolNaam;
                member.getProperty("straatschool_1").Value = model.School1.Street;
                member.getProperty("nrSchool_1").Value = model.School1.Nr;
                member.getProperty("busschool_1").Value = model.School1.Box;
                member.getProperty("zipcodeschool_1").Value = model.School1.Zip;
                member.getProperty("plaatsschool_1").Value = model.School1.Location;
                member.getProperty("telschool_1").Value = model.School1.Tel;
                member.getProperty("emailschool_1").Value = model.School1.Email;


                // school 2
                member.getProperty("naamschool_2").Value = model.School2.SchoolNaam;
                member.getProperty("straatschool_2").Value = model.School2.Street;
                member.getProperty("nrschool_2").Value = model.School2.Nr;
                member.getProperty("busschool_2").Value = model.School2.Box;
                member.getProperty("zipcodeschool_2").Value = model.School2.Zip;
                member.getProperty("plaatsschool_2").Value = model.School2.Location;
                member.getProperty("telschool_2").Value = model.School2.Tel;
                member.getProperty("emailschool_2").Value = model.School2.Email;
                member.Save();

                return "ok";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public ReturnResult ChangeCredentials(int id)
        {
            try
            {
                var member = new Member(id);
                // member.LoginName = id
                // member.getProperty("naam").Value = model.LastName;
                // member.getProperty("voornaam").Value = model.FirstName;
                // member.LoginName = model.Email;
                member.Text = string.Format("{0} {1}",
                    member.getProperty("naam").Value,
                    member.getProperty("voornaam").Value);

                member.LoginName = member.getProperty("lidnr").Value.ToString();

                member.Save();

                member.XmlGenerate(new XmlDocument());

                return new ReturnResult
                {
                    Success = true,
                    Message = member.LoginName
                };
                //return "ok";
            }
            catch (Exception ex)
            {
                return new ReturnResult
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public void UpdateMember(MemberProfileViewModel model)
        {
            try
            {
                var member = new Member(model.Id);

                member.getProperty("naam").Value = model.LastName;
                member.getProperty("voornaam").Value = model.FirstName;
                member.getProperty("straat").Value = model.Street;
                member.getProperty("nr").Value = model.Nr;
                member.getProperty("bus").Value = model.Box;
                member.getProperty("postcode").Value = model.Zip;
                member.getProperty("plaats").Value = model.Location;
                member.getProperty("tel").Value = model.Tel;
                member.getProperty("mobile").Value = model.Mobile;
                member.getProperty("country").Value = model.Country;
                member.getProperty("gender").Value =
                    ((Gender) Convert.ToInt32(model.rbGenderOptions.SelectedValue)).ToString();
                member.getProperty("profiel").Value = ((BvlfMemberProfile) model.MemberProfile).ToString();

                // school 1
                member.getProperty("naamschool_1").Value = model.School1.SchoolNaam;
                member.getProperty("straatschool_1").Value = model.School1.Street;
                member.getProperty("nrSchool_1").Value = model.School1.Nr;
                member.getProperty("busschool_1").Value = model.School1.Box;
                member.getProperty("zipcodeschool_1").Value = model.School1.Zip;
                member.getProperty("plaatsschool_1").Value = model.School1.Location;
                member.getProperty("telschool_1").Value = model.School1.Tel;
                member.getProperty("emailschool_1").Value = model.School1.Email;


                // school 2
                member.getProperty("naamschool_2").Value = model.School2.SchoolNaam;
                member.getProperty("straatschool_2").Value = model.School2.Street;
                member.getProperty("nrschool_2").Value = model.School2.Nr;
                member.getProperty("busschool_2").Value = model.School2.Box;
                member.getProperty("zipcodeschool_2").Value = model.School2.Zip;
                member.getProperty("plaatsschool_2").Value = model.School2.Location;
                member.getProperty("telschool_2").Value = model.School2.Tel;
                member.getProperty("emailschool_2").Value = model.School2.Email;

                member.Save();
            }
            catch (Exception ex)
            {
            }
        }

        public bool ChangePassword(ChangePasswordViewModel model)
        {
            var member = Member.GetCurrentMember();
            var u = Membership.GetUser(member.Email);
            return u.ChangePassword(member.Password, model.Password);
        }

        public bool MemberExists(string email)
        {
            try
            {
                var u = Membership.GetUser(email);
                return u != null;
            }
            catch
            {
                return false;
            }
        }

        public List<cmsMember> GetAllMembers()
        {
            return _context.cmsMembers.ToList();
        }

        public cmsMember GetCmsMemberById(int memberid)
        {
            return _context.cmsMembers.FirstOrDefault(p => p.nodeId == memberid);
        }

        public string RetrievePassword(string email)
        {
            var member = Member.GetMemberFromEmail(email);
            return member.Password;
        }

        public void SetMemberShipPaid(int memberid)
        {
            var member = new Member(memberid);
            member.getProperty("bvlfMemberStatus").Value = (int) BvlfMemberStatus.Betaald;
            member.Save();

            member.XmlGenerate(new XmlDocument());
            Member.AddMemberToCache(member);
        }

        public void SetMembershipToSleep(int memberid)
        {
            var member = new Member(memberid);
            member.getProperty("bvlfMemberStatus").Value = (int) BvlfMemberStatus.Verwijderd;
            member.Save();

            member.XmlGenerate(new XmlDocument());
            Member.AddMemberToCache(member);
        }

        public void RenewMembership(int id)
        {
            var member = new Member(id);
            var currentExpirationDate =
                DateTime.Parse(ViewModelHelper.GetStringValue(member.getProperty("subscriptionExpiration")));
            var renewedExpirationDate = currentExpirationDate.AddYears(1);
            member.getProperty("subscriptionExpiration").Value = renewedExpirationDate;

            member.getProperty("bvlfMemberStatus").Value = (int) BvlfMemberStatus.Ingeschreven;

            member.Save();

            member.XmlGenerate(new XmlDocument());
            Member.AddMemberToCache(member);
        }

        public void SetRenewSubscriptionMailSent(int id)
        {
            var member = new Member(id);
            member.getProperty("renewalSubscriptionReminder").Value = 1;
            member.Save();
            member.XmlGenerate(new XmlDocument());
        }
    }

    public class ReturnResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}