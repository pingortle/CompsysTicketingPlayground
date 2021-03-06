﻿using AdmitOne.Domain.Entities;
using AdmitOne.Persistence;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;

namespace AdmitOne.ViewModel
{
    public sealed class DispatchViewModel : ReactiveObject, IRoutableViewModel
    {
        public DispatchViewModel(IScreen screen, ISession session)
        {
            HostScreen = screen;
            GoBack = HostScreen.Router.NavigateBack;

            Techs = new ReactiveList<Employee>();
            Tickets = new ReactiveList<Ticket>();

            var getFreshTechs = new ReactiveCommand();
            getFreshTechs.ObserveOn(RxApp.MainThreadScheduler).Subscribe(_ =>
                {
                    Techs.Clear();
                    session.FetchResults<Employee>()
                        .ObserveOn(RxApp.MainThreadScheduler)
                        .Subscribe(x => Techs.Add(x));
                });

            var getFreshTickets = new ReactiveCommand();
            getFreshTickets.ObserveOn(RxApp.MainThreadScheduler).Subscribe(_ =>
                {
                    Tickets.Clear();
                    session.FetchResults<Ticket>()
                        .ObserveOn(RxApp.MainThreadScheduler)
                        .Subscribe(x => Tickets.Add(x));
                });

            Refresh = new ReactiveCommand(session.IsWorking.Select(x => !x));
            Refresh.Subscribe(_ =>
                {
                    getFreshTechs.Execute(default(object));
                    getFreshTickets.Execute(default(object));
                });

            Assign = new ReactiveCommand(Observable.CombineLatest(
                this.WhenAny(
                    x => x.SelectedEmployee,
                    y => y.SelectedTicket,
                    (x, y) => x.Value != null && y.Value != null),
                Refresh.CanExecuteObservable,
                (x, y) => x && y));
            Assign.Subscribe(_ =>
            {
                using (session.ScopedChanges())
                {
                    var eventTaker = session.Take<TicketEvent>();
                    eventTaker.Add(new TicketEvent { Employee = SelectedEmployee, Ticket = SelectedTicket, TicketStatus = TicketStatus.Assigned, Time = DateTime.Now });
                }
            });

            _error = session.ThrownExceptions
                .Select(x => x.Message)
                .ObserveOn(RxApp.MainThreadScheduler)
                .ToProperty(this, x => x.Error);

            Refresh.Execute(default(object));
        }

        public IReactiveCollection<Employee> Techs { get; private set; }
        public IReactiveCollection<Ticket> Tickets { get; private set; }

        private Employee _selectedEmployee;
        public Employee SelectedEmployee
        {
            get { return _selectedEmployee; }
            set { this.RaiseAndSetIfChanged(ref _selectedEmployee, value); }
        }

        private Ticket _selectedTicket;
        public Ticket SelectedTicket
        {
            get { return _selectedTicket; }
            set { this.RaiseAndSetIfChanged(ref _selectedTicket, value); }
        }

        public IReactiveCommand GoBack { get; private set; }
        public IReactiveCommand Refresh { get; private set; }
        public IReactiveCommand Assign { get; private set; }

        private ObservableAsPropertyHelper<string> _error;
        public string Error { get { return _error.Value; } }

        public IScreen HostScreen { get; private set; }

        public string UrlPathSegment { get { return "dispatch"; } }
    }
}
