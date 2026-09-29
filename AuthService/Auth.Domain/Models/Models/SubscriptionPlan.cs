using Auth.Domain.Enums;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;

namespace Auth.Domain.Models.Models
{
    public class SubscriptionPlan
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public SubscriptionPlanCode Code { get; set; }
        public decimal Price { get; set; }
        public int DurationDays { get; set; }
        public string? Description { get; set; }
        public bool IsPopular { get; set; }


        public SubscriptionPlan() { }

        public SubscriptionPlan(int id, string name, SubscriptionPlanCode code, decimal price, int durationDays, string? description, bool isPopular)
        {
            Id = id;
            Name = name;
            Code = code;
            Price = price;
            DurationDays = durationDays;
            Description = description;
            IsPopular = isPopular;
        }

    }
}
