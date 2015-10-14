using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_DMD.Classes
{
    public sealed class FakeGenerator
    {
        static readonly FakeGenerator _instance = new FakeGenerator();

        public static FakeGenerator Instance
        {
            get
            {
                return _instance;
            }
        }

        public FakeDataRepository FakeRepository { get; set; }
        public FakeAppUserRepository FakeUsersRepository { get; set; }
        private FakeGenerator()
        {
            FakeRepository = new FakeDataRepository();
            FakeUsersRepository = new FakeAppUserRepository();
        }
    }
}