﻿using System.Collections.Generic;
using System.IO;
using FluentAssertions;
using Nett.Tests.Util;
using Xunit;

namespace Nett.Coma.Tests.Functional
{
    public sealed class AutoConversions
    {
        public class AppSettings
        {
            public ConnectionStrings CS { get; set; } = new ConnectionStrings();

            public class ConnectionStrings
            {
                public string con { get; set; }
            }

            public UserSettings User { get; set; } = new UserSettings();

            public class UserSettings
            {
                public Dictionary<string, bool> Items { get; set; }
            }
        }


        [Fact]
        public void Foo()
        {
            using (var fn = TestFileName.Create("input", ".toml"))
            {
                // Arrange
                const string tml = @"
[User]

[User.Items]
first = true
second = false
third = true
";
                File.WriteAllText(fn, tml);

                // Act
                var cfg = Config.CreateAs()
                    .MappedToType(() => new AppSettings())
                    .StoredAs(store => store.File(fn))
                    .Initialize();
                var u = Toml.ReadString<AppSettings>(tml);

                // Assert
                cfg.Get(c => c.User.Items).Count.Should().Be(3);
            }
        }
    }
}
