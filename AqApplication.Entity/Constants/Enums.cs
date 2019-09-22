using System.ComponentModel.DataAnnotations;

namespace AqApplication.Entity.Constants
{
     public enum MemberType
    {
        [Display(Name = "MemberType.Admin")]
        Admin = 0,
        [Display(Name = "MemberType.Agent")]
        Agent =1,
        [Display(Name = "MemberType.WebUser")]
        WebUser =2,
        [Display(Name = "MemberType.Advisor")]
        Advisor =3
    }

    public enum ChallengeTypeEnum
    {
        [Display(Name = "ChallengeTypeEnum.RandomMode")]
        RandomMode =1,
        [Display(Name = "ChallengeTypeEnum.PracticeMode")]
        PracticeMode =2
    }
}