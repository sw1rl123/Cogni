﻿using System;
using System.Collections.Generic;
// using Cogni.Models;

namespace Cogni.Database.Entities;
public class Message
{
    public int MessageId { get; set; }
    public Guid ChatId { get; set; }
    public int SenderId { get; set; }
    public string Msg { get; set; } = string.Empty;
    public DateTime Date { get; set; } = DateTime.UtcNow;
    public bool IsEdited { get; set; } = false;
    public bool IsFunctional { get; set; } = false;
    public List<string>? Attachments { get; set; }
    public List<MessageStatus> MessageStatuses { get; set; } = new();
    public static Message ChatCreated(Guid chatId, int userId) {
        return new Message
        {
            ChatId = chatId,
            SenderId = userId,
            Msg = "Chat created", 
            Date = DateTime.UtcNow,
            IsEdited = false,
            Attachments = null,
            IsFunctional = true 
        };
    }

    public static Message UserLeaved(Guid chatId, int userId) {
        return new Message
        {
            ChatId = chatId,
            SenderId = userId,
            Msg = $"[{userId}] left chat", 
            Date = DateTime.UtcNow,
            IsEdited = false,
            Attachments = null,
            IsFunctional = true 
        };
    }

    public static Message UserAdded(Guid chatId, int userId, string invoker) {
        return new Message
        {
            ChatId = chatId,
            SenderId = userId,
            Msg = $"[{invoker}] invited [{userId}] to chat", 
            Date = DateTime.UtcNow,
            IsEdited = false,
            Attachments = null,
            IsFunctional = true 
        };
    }

    public static Message GroupRenamed(Guid chatId, int userId, string name) {
        return new Message
        {
            ChatId = chatId,
            SenderId = userId,
            Msg = $"[{userId}] renamed this group to {name}", 
            Date = DateTime.UtcNow,
            IsEdited = false,
            Attachments = null,
            IsFunctional = true
        };
    }
}

public class MessageStatus
{
    public int UserId { get; set; }
    public Guid ChatId { get; set; }
    public int LastReaden { get; set; }
}




