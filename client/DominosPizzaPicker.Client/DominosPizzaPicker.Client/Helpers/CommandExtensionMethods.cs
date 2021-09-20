using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DominosPizzaPicker.Client.Helpers
{
    public static class CommandExtensionMethods
    {

        public static Command CreateCommand<T>(this CustomViewModel vm, Action<T> execute, Func<T, bool> canExecute = null, List<string> canExecuteProperties = null)
        {
            var com = new Command<T>((a) =>
                {
                    vm.IsBusy = true;
                    execute(a);
                    vm.IsBusy = false;
                },
                (a) =>
                {
                    var result = true;
                    if (canExecute != null)
                        result = canExecute(a);
                    return result && !vm.IsBusy;
                });

            if (canExecuteProperties == null)
                canExecuteProperties = new List<string>();

            if (!canExecuteProperties.Contains(nameof(vm.IsBusy)))
                canExecuteProperties.Add(nameof(vm.IsBusy));

            vm.CommandCanExecuteProperties.Add(com, canExecuteProperties);

            return com;
        }

        public static Command CreateCommand<T>(this CustomViewModel vm, Func<T, Task> execute, Func<T, bool> canExecute = null, List<string> canExecuteProperties = null)
        {
            var com = new Command<T>(async (a) =>
                {
                    vm.IsBusy = true;
                    await execute(a);
                    vm.IsBusy = false;
                },
                (a) =>
                {
                    var result = true;
                    if (canExecute != null)
                        result = canExecute(a);
                    return result && !vm.IsBusy;
                });

            if (canExecuteProperties == null)
                canExecuteProperties = new List<string>();

            if (!canExecuteProperties.Contains(nameof(vm.IsBusy)))
                canExecuteProperties.Add(nameof(vm.IsBusy));

            vm.CommandCanExecuteProperties.Add(com, canExecuteProperties);

            return com;
        }



        public static Command CreateCommand(this CustomViewModel vm, Action<object> execute, Func<object, bool> canExecute = null, List<string> canExecuteProperties = null)
        {
            var com = new Command(a =>
                {
                    vm.IsBusy = true;
                    execute(a);
                    vm.IsBusy = false;
                },
                a =>
                {
                    var result = true;
                    if (canExecute != null)
                        result = canExecute(a);
                    return result && !vm.IsBusy;
                });

            if (canExecuteProperties == null)
                canExecuteProperties = new List<string>();

            if(!canExecuteProperties.Contains(nameof(vm.IsBusy)))
                canExecuteProperties.Add(nameof(vm.IsBusy));

            vm.CommandCanExecuteProperties.Add(com, canExecuteProperties);

            return com;
        }


        public static Command CreateCommand(this CustomViewModel vm, Func<object, Task> execute, Func<object, bool> canExecute = null, List<string> canExecuteProperties = null)
        {
            var com = new Command(async (a) =>
                {
                    vm.IsBusy = true;
                    await execute(a);
                    vm.IsBusy = false;
                },
                a =>
                {
                    var result = true;
                    if (canExecute != null)
                        result = canExecute(a);
                    return result && !vm.IsBusy;
                });

            if (canExecuteProperties == null)
                canExecuteProperties = new List<string>();

            if (!canExecuteProperties.Contains(nameof(vm.IsBusy)))
                canExecuteProperties.Add(nameof(vm.IsBusy));

            vm.CommandCanExecuteProperties.Add(com, canExecuteProperties);

            return com;
        }



        public static Command CreateCommand(this CustomViewModel vm, Action execute, Func<bool> canExecute = null, List<string> canExecuteProperties = null)
        {
            var com = new Command(() =>
                {
                    vm.IsBusy = true;
                    execute();
                    vm.IsBusy = false;

                },
                 () =>
                {
                    var result = true;
                    if (canExecute != null)
                        result = canExecute();
                    return result && !vm.IsBusy;
                });


            if (canExecuteProperties == null)
                canExecuteProperties = new List<string>();

            if (!canExecuteProperties.Contains(nameof(vm.IsBusy)))
                canExecuteProperties.Add(nameof(vm.IsBusy));

            vm.CommandCanExecuteProperties.Add(com, canExecuteProperties);

            return com;
        }
        public static Command CreateCommand(this CustomViewModel vm, Func<Task> execute, Func<bool> canExecute = null, List<string> canExecuteProperties = null)
        {
            var com = new Command(async () =>
                {
                    vm.IsBusy = true;
                    await execute();
                    vm.IsBusy = false;

                },
                 () =>
                {
                    var result = true;
                    if (canExecute != null)
                        result = canExecute();
                    return result && !vm.IsBusy;

                });


            if (canExecuteProperties == null)
                canExecuteProperties = new List<string>();

            if (!canExecuteProperties.Contains(nameof(vm.IsBusy)))
                canExecuteProperties.Add(nameof(vm.IsBusy));

            vm.CommandCanExecuteProperties.Add(com, canExecuteProperties);

            return com;
        }

    }
}