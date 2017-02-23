﻿using System.Collections;
using System.Collections.Generic;

namespace EHVAG.MusiGModel
{
    public class User
    {
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string UserName { get; set; }

        public virtual ICollection<Channel> Channels { get; set; } = new List<Channel>();
    }
}