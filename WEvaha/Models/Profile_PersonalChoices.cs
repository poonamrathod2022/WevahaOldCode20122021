using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WEvaha.Models
{
    public class Profile_PersonalChoices
    {
        public int Goals { get; set; }
        public int FriendsNetwork { get; set; }
        public int FriendLaugh { get; set; }
        public int HouseImprovements { get; set; }

        public int CareAbout { get; set; }
        public int OrganizedLife { get; set; }
        public int Finances { get; set; }
        public int HomeEntertaining { get; set; }
        public int CaringforChildren { get; set; }
        public int RomanceinRelation { get; set; }
        public int Socializing { get; set; }
        public int HomeEnvironment { get; set; }
        public int SharingBeliefs { get; set; }
        public int PhysicalFit { get; set; }
        public int CalmDuringCrisis { get; set; }
        public int ThoughtsAndfeelings { get; set; }
        public int HelpingFortunates { get; set; }
        public int ResolveConflict { get; set; }
        public int Adventures { get; set; }
        public int KnowledgeAndAwareness { get; set; }
        public int ProfessionalPlanning { get; set; }
        public int EventsUnderstanding { get; set; }
        public int FindingPleasure { get; set; }
        public int CultureAndTradition { get; set; }
        public int CreativeSolutions { get; set; }
        public int MakingFriends { get; set; }
        public int CookingForFamily { get; set; }
        public int ProvideIncomeforFamily { get; set; }
        public int HouseholdSchedules { get; set; }
        public int BeingAgoodFriend { get; set; }

        public List<SelectListItem> GoalsOpti { get; set; }
        public List<SelectListItem> FriendsNetworkOpti { get; set; }
        public List<SelectListItem> FriendLaughOpti { get; set; }
        public List<SelectListItem> HouseImprovementsOpti { get; set; }

        public List<SelectListItem> CareAboutOpti { get; set; }
        public List<SelectListItem> OrganizedLifeOpti { get; set; }
        public List<SelectListItem> FinancesOpti { get; set; }
        public List<SelectListItem> HomeEntertainingOpti { get; set; }
        public List<SelectListItem> CaringforChildrenOpti { get; set; }
        public List<SelectListItem> RomanceinRelationOpti { get; set; }
        public List<SelectListItem> SocializingOpti { get; set; }
        public List<SelectListItem> HomeEnvironmentOpti { get; set; }
        public List<SelectListItem> SharingBeliefsOpti { get; set; }
        public List<SelectListItem> PhysicalFitOpti { get; set; }
        public List<SelectListItem> CalmDuringCrisisOpti { get; set; }
        public List<SelectListItem> ThoughtsAndfeelingsOpti { get; set; }
        public List<SelectListItem> HelpingFortunatesOpti { get; set; }
        public List<SelectListItem> ResolveConflictOpti { get; set; }
        public List<SelectListItem> AdventuresOpti { get; set; }
        public List<SelectListItem> KnowledgeAndAwarenessOpti { get; set; }
        public List<SelectListItem> ProfessionalPlanningOpti { get; set; }
        public List<SelectListItem> EventsUnderstandingOpti { get; set; }
        public List<SelectListItem> FindingPleasureOpti { get; set; }
        public List<SelectListItem> CultureAndTraditionOpti { get; set; }
        public List<SelectListItem> CreativeSolutionsOpti { get; set; }
        public List<SelectListItem> MakingFriendsOpti { get; set; }
        public List<SelectListItem> CookingForFamilyOpti { get; set; }
        public List<SelectListItem> ProvideIncomeforFamilyOpti { get; set; }
        public List<SelectListItem> HouseholdSchedulesOpti { get; set; }
        public List<SelectListItem> BeingAgoodFriendOpti { get; set; }
    }
}