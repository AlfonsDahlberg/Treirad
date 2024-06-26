﻿using System;
using System.Windows.Input;

namespace TreIrad
{
    /// <summary>
    /// Används inte nu men kan testas i framtiden.
    /// Innehåller kod för command som kan användas för att koppla klick events till spelmotorn på ett annat sätt.
    /// </summary>
    public class RelayCommand : ICommand
    {
        private readonly Predicate<object?> canExecute;
        private readonly Action<object?> execute;

        public RelayCommand(Action<object?> execute, Predicate<object?> canExecute)
        {
            this.canExecute = canExecute;
            this.execute = execute;
        }

        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public bool CanExecute(object? parameter)
        {
            return canExecute(parameter);
        }

        
        public void Execute(object? parameter)
        {
            execute(parameter);
        }
    }
}
