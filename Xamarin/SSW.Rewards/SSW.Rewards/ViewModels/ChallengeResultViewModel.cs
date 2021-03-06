﻿using SSW.Rewards.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SSW.Rewards.ViewModels
{
    public class ChallengeResultViewModel
    {
        public int Points { get; set; }
        public string Title { get; set; }
        public ChallengeResult result { get; set; }
        public ChallengeType ChallengeType { get; set; }
    }
}
