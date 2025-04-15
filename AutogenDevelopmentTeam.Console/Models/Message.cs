using System;

namespace AutogenDevelopmentTeam.Console.Models
{
    public class Message
    {
        public Role Sender { get; set; }
        public string Content { get; set; }

        public Message(Role sender, string content)
        {
            Sender = sender;
            Content = content;
        }
    }

    public enum Role
    {
        User,
        Developer,
        UnitTester
    }
}