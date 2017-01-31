using GenomeNext.Cloud.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenomeNext.App.Console
{
    public interface IConsoleApp
    {
        void Init();

        void Run();
    }
}
