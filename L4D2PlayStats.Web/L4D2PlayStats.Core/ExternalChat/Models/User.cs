﻿using L4D2PlayStats.Core.Auth;

namespace L4D2PlayStats.Core.ExternalChat.Models;

public class User
{
    public User()
    {
    }

    public User(ICurrentUser currentUser)
    {
        SteamId = currentUser.SteamId;
        CommunityId = currentUser.CommunityId;
        Steam3 = currentUser.Steam3;
        ProfileUrl = currentUser.ProfileUrl;
        Name = currentUser.Name;
        AvatarUrl = currentUser.AvatarUrl;
    }

    public string? SteamId { get; set; }
    public string? CommunityId { get; set; }
    public string? Steam3 { get; set; }
    public string? ProfileUrl { get; set; }
    public string? Name { get; set; }
    public string? AvatarUrl { get; set; }
}