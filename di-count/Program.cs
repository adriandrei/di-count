using Microsoft.Extensions.DependencyInjection;
using System;

namespace di_count
{
    class Program
    {
        static void Main(string[] args)
        {
            var transientServiceProvider = new ServiceCollection()
                .AddTransient<IFooService, FooService>()
                .AddTransient<IBarService, BarService>()
                .BuildServiceProvider();

            var singletonServiceProvider = new ServiceCollection()
                .AddSingleton<IFooService, FooService>()
                .AddSingleton<IBarService, BarService>()
                .BuildServiceProvider();

            var count = 10;
            int i = 0;
            Console.WriteLine($"Should print ctor message for each dependency, every time we fetch an instance");
            while (i++ < count)
                transientServiceProvider.GetService<IBarService>();


            i = 0;
            Console.WriteLine($"Should print ctor message for each dependency, just once");
            while (i++ < count)
                singletonServiceProvider.GetService<IBarService>();

            Console.ReadKey();
        }
    }

    public interface IBarService
    {
        void Work();
    }

    public interface IFooService
    {
        void Work(int number);
    }

    public class FooService : IFooService
    {
        public FooService()
        {
            Console.WriteLine($"Instance of {nameof(FooService)}");
        }

        public void Work(int number)
        {
            Console.WriteLine($"Printing {number}");
        }
    }

    public class BarService : IBarService
    {
        private readonly IFooService fooService;
        public BarService(IFooService fooService)
        {
            Console.WriteLine($"Instance of {nameof(BarService)}");
            this.fooService = fooService ?? throw new ArgumentNullException(nameof(FooService));
        }

        public void Work()
        {
            for (int i = 0; i < 10; i++)
            {
                fooService.Work(i);
            }
        }
    }

}
