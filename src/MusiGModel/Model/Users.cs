﻿using System.Collections;
using System.Collections.Generic;

namespace MusiGModel.Model
{
    public class User
    {
        public User()
        {
            UserPlaylists = new List<UserPlaylist>();
            UserAccounts = new List<UserAccount>();
        }

        public int UserId { get; set; }
        public string Forname { get; set; }
        public string Lastname { get; set; }
        public string UserName { get; set; }

        public virtual ICollection UserPlaylists { get; set; }
        public virtual ICollection UserAccounts { get; set; }
    }
}