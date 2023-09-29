﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TwitterAuth_Backend.Model
{
    public class UserInfoModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        
        public long UserKey { get; set; }
        public string Token { get; set; }
        public string TokenSecret { get; set; }
        public string UserId { get; set; }
        public string Username { get; set; }

    }
}
