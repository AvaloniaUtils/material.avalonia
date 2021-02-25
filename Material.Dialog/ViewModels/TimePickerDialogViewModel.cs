﻿using Avalonia.Layout;
using Avalonia.Threading;
using Material.Dialog.Commands;
using Material.Dialog.ViewModels.TextField;
using Material.Dialog.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Avalonia.Media;

namespace Material.Dialog.ViewModels
{
    public class TimePickerDialogViewModel : DialogWindowViewModel
    {
        private TimePickerDialog _window;

        private DialogResultButton[] m_DialogButtons;
        public DialogResultButton[] DialogButtons { get => m_DialogButtons; internal set => m_DialogButtons = value; }

        private DialogResultButton m_PositiveButton;
        public DialogResultButton PositiveButton { get => m_PositiveButton; internal set => m_PositiveButton = value; }

        private DialogResultButton m_NegativeButton;
        public DialogResultButton NegativeButton { get => m_NegativeButton; internal set => m_NegativeButton = value; }

        private ushort _firstField = 0;
        public ushort FirstField
        {
            get => _firstField;
            set
            {
                _firstField = value;
                OnPropertyChanged();
            }
        }
        
        private ushort _secondField = 0;
        public ushort SecondField
        {
            get => _secondField;
            set
            {
                _secondField = value;
                OnPropertyChanged();
            }
        }

        private string _firstPanelPointerTransform;
        public string FirstPanelPointerTransform
        {
            get => _firstPanelPointerTransform;
            set
            {
                _firstPanelPointerTransform = value;
                OnPropertyChanged();
            }
        }
        
        private string _secondPanelPointerTransform;
        public string SecondPanelPointerTransform
        {
            get => _secondPanelPointerTransform;
            set
            {
                _secondPanelPointerTransform = value;
                OnPropertyChanged();
            }
        }

        private bool _isAm = true;
        public bool IsAm
        {
            get => _isAm;
            set
            {
                _isAm = value;
                OnPropertyChanged();
            }
        }

        private int _carouselIndex = 0;
        public int CarouselIndex
        {
            get => _carouselIndex;
            set
            {
                _carouselIndex = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(FirstFieldSelected));
                OnPropertyChanged(nameof(SecondFieldSelected));
            }
        }

        public bool FirstFieldSelected
        {
            get => _carouselIndex == 0;
            set
            { 
                CarouselIndex = value ? 0 : 1; 
                OnPropertyChanged();
            }
        }
        
        public bool SecondFieldSelected
        {
            get => _carouselIndex == 1;
            set
            {
                CarouselIndex = value ? 1 : 0;
                OnPropertyChanged();
            }
        }

        public void SetFirstField(int v)
        {
            if (v == FirstField)
                return;
            
            FirstField = (ushort)v;
            FirstPanelPointerTransform = $"rotate({(v / (double)12) * 360}deg)";
        }
        
        public void SetSecondField(int v)
        {
            if (v == SecondField)
                return;
            
            SecondField = (ushort)v;
            var r = Math.Round((v / (double) 60) * 360);
            SecondPanelPointerTransform = $"rotate({r}deg)";
        }

        public TimePickerDialogViewModel(TimePickerDialog dialog)
        {
            _window = dialog;
            ButtonClick = new RelayCommand(OnPressButton, CanPressButton);
        }

        public bool ValidateFields()
        {
            return true;
        }

        public bool CanPressButton(object args) => true;
        public async void OnPressButton(object args)
        {
            var button = args as DialogResultButton;
            if (button is null)
                return; 

            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                var timespan = new TimeSpan(FirstField + (_isAm ? 0 : 12), SecondField, 00);
                
                var result = new DateTimePickerDialogResult(button.Result, timespan);
                var fields = new List<TextFieldResult>();

                _window.Result = result;
                _window.Close();
            });
        }

        public RelayCommand ButtonClick { get; private set; }
    }
}
