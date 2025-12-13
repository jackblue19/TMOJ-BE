using Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

public enum Difficulty { Easy = 1, Medium = 2, Hard = 3 }

public sealed class Problem
{
    public Guid Id { get; private set; }
    public string Title { get; private set; } = null!;
    public Slug Slug { get; private set; } = null!;
    public Difficulty Difficulty { get; private set; }
    public bool IsPublic { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }

    private Problem() { }

    // đó ngoài scaffold ra thì cần thêm cái constructor nữa nha
    public Problem(Guid id , string title , Slug slug , Difficulty difficulty , bool isPublic , DateTime createdAtUtc)
    {
        Id = id;
        Title = title;
        Slug = slug;
        Difficulty = difficulty;
        IsPublic = isPublic;
        CreatedAtUtc = createdAtUtc;
    }

    public void Publish() => IsPublic = true;

    //  factory method      (factory design pattern)
    public static Problem Create(
                                string title ,
                                Slug slug ,
                                Difficulty difficulty ,
                                bool isPublic)
    {
        return new Problem(
            Guid.NewGuid() ,
            title ,
            slug ,
            difficulty ,
            isPublic ,
            DateTime.UtcNow
        );
    }

}
