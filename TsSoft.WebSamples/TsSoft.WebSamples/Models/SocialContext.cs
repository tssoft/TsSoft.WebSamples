﻿namespace TsSoft.WebSamples.Models
{
    using System.Data.Entity;
    using TsSoft.Social.Facebook;
    using TsSoft.Social.Twitter;
    using TsSoft.Social.Vk;

    public class SocialContext : DbContext
    {
        public DbSet<VkUser> VkUser { get; set; }
        public DbSet<FacebookUser> FacebookUser { get; set; }
        public DbSet<TwitterUser> TwitterUser { get; set; }
    }
}