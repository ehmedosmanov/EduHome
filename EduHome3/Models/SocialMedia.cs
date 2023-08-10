using System;
using System.Collections.Generic;

namespace EduHome3.Models
{
    public class SocialMedia
    {
        public int Id { get; set; }
        public string MediaIcon { get; set; }
        public string MediaLink { get; set; }

        public static implicit operator List<object>(SocialMedia v)
        {
            throw new NotImplementedException();
        }
    }
}
