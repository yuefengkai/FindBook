using System;

namespace User.Api.Models
{
    /// <summary>
    /// 用户标签
    /// </summary>
    public class UserTag
    {

        public int AppUserId { get; set; }

        public string Tag { get; set; }

        public DateTime CreateTime { get; set; }
    }

}
