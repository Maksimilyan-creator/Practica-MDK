using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using MySqlConnector;
using Practica_Lamaev.Models;

namespace Practica_Lamaev;

public partial class window : Window
{
    private List<Appointments> AppointmentsList { get; set; }
    private Appointments New_Appointment { get; set; }
    private List<Patients> PatientsList { get; set; }
    private List<Patients> NewPatientsList { get; set; }
    private List<Doctors> DoctorsList { get; set; }
    private List<Doctors> NewDoctorsList { get; set; }
    private List<Diagnosis> DiagnosisList { get; set; }
    private List<Diagnosis> NewDiagnosisList { get; set; }
    private List<Payments> PaymentsList { get; set; }
    private List<Payments> NewPaymentsList { get; set; }
    private List<Recovery> RecoveryList { get; set; }
    private List<Recovery> NewRecoveryList { get; set; }
    private List<Medical_card> MedicalcardList { get; set; }
    private MySqlConnectionStringBuilder _connectionSb;

    public window()
    {
        InitializeComponent();
        AppointmentsList = new List<Appointments>();
        
        PatientsList = new List<Patients>();
        DoctorsList = new List<Doctors>();
        DiagnosisList = new List<Diagnosis>();
        PaymentsList = new List<Payments>();
        RecoveryList = new List<Recovery>();
        MedicalcardList = new List<Medical_card>();
        _connectionSb = new MySqlConnectionStringBuilder
        {
            Server = "10.10.1.24",
            Database = "pro3",
            UserID = "user_01",
           Password = "user01pro"
             // Server = "localhost",
             // Database = "practica_lamaev",
             // UserID = "root",
             // Password = "123456"
        };
       UpdateAppointments();
        UpdatePatients();
        UpdateDoctors();
        UpdateDiagnosis();
        UpdatePayments();
        UpdateRecovery();
        UpdateMedical_card();
    }
    public void UpdateAppointments()
    {
        AppointmentsList.Clear();
        using (var connection = new MySqlConnection(_connectionSb.ConnectionString))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText =
                    "SELECT * FROM Appointments JOIN Patients ON Appointments.Patient = Patients.ID " +
                    "JOIN Doctors ON Appointments.Doctor = Doctors.ID " +
                    "JOIN Payments ON Appointments.Payment_Status = Payments.ID " +
                    "JOIN Diagnosis ON Appointments.Diagnosis =Diagnosis.ID " +
                    "JOIN Recovery ON Appointments.Recovery = Recovery.ID";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        AppointmentsList.Add(new Appointments()
                        {
                            Patient = reader.GetString("LastName"),
                            Doctor = reader.GetString("LastName_"),
                            Payment_Status = reader.GetBoolean("Status"),
                            Diagnosis = reader.GetString("Name_Diagnosis"),
                            Treatmen = reader.GetString("Treatmen"),
                            Recovery = reader.GetInt32("Recovery_indicators"),
                            Date = reader.GetDateTime("Date"),
                            Attendance = reader.GetBoolean("Attendance"),
                        });
                    }
                }

            }

            connection.Close();
        }

        AppointmentDataGrid.ItemsSource = AppointmentsList;
        
    }

    void UpdatePatients()
    {
        
        using (var connection = new MySqlConnection(_connectionSb.ConnectionString))
        {
            
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * From Patients";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        PatientsList.Add(new Patients()
                        {
                            ID = reader.GetInt32("id"),
                            LastName = reader.GetString("LastName"),
                            FirstName = reader.GetString("FirstName"),
                            Namber = reader.GetString("Namber"),
                            Mail = reader.GetString("Mail"),
                            DOB = reader.GetDateTime("DOB")
                        });
                    }
                }

            }
            connection.Close();
        }
        PatientsDataGrid.ItemsSource = PatientsList;
    }
    
void UpdateDoctors()
    {
        using (var connection = new MySqlConnection(_connectionSb.ConnectionString))
        {
            
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * From Doctors";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DoctorsList.Add(new Doctors()
                        {
                            ID = reader.GetInt32("id"),
                            LastName_ = reader.GetString("LastName_"),
                            FirstName = reader.GetString("FirstName"),
                            Namber = reader.GetString("Namber"),
                            Mail = reader.GetString("Mail")
                        });
                    }
                }

            }

            connection.Close();
        }

        DoctorsDataGrid.ItemsSource = DoctorsList;
    }

    void UpdateDiagnosis()
    {
        
        using (var connection = new MySqlConnection(_connectionSb.ConnectionString))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * From Diagnosis";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DiagnosisList.Add(new Diagnosis()
                        {
                            ID = reader.GetInt32("id"),
                            Name_Diagnosis = reader.GetString("Name_Diagnosis")
                        });
                    }
                }

            }

            connection.Close();
        }

        DiagnosisDataGrid.ItemsSource = DiagnosisList;
        DiagnosisFilterBox.ItemsSource = DiagnosisList;
    }

    void UpdatePayments()
    {
        
        using (var connection = new MySqlConnection(_connectionSb.ConnectionString))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * From Payments";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        PaymentsList.Add(new Payments()
                        {
                            ID = reader.GetInt32("id"),
                            Patient = reader.GetInt32("Patient"),
                            Amount = reader.GetInt32("Amount"),
                            Status = reader.GetBoolean("Status"),
                            Date = reader.GetDateTime("Date")
                        });
                    }
                }

            }

            connection.Close();
        }

        PaymensDataGrid.ItemsSource = PaymentsList;
    }

    void UpdateRecovery()
    {
        
        using (var connection = new MySqlConnection(_connectionSb.ConnectionString))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * From Recovery";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        RecoveryList.Add(new Recovery()
                        {
                            ID = reader.GetInt32("id"),
                            Patient = reader.GetInt32("Patient"),
                            Recovery_indicators = reader.GetInt32("Recovery_indicators"),
                            Expected_date_of_discharge = reader.GetDateTime("Expected_date_of_discharge")
                        });
                    }
                }

            }

            connection.Close();
        }

        RecoveryDataGrid.ItemsSource = RecoveryList;
    }
    public void UpdateMedical_card()
    {
        using (var connection = new MySqlConnection(_connectionSb.ConnectionString))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * From Medical_card JOIN Patients ON Medical_card.Patient = Patients.ID " +
                                      "JOIN Appointments ON Medical_card.Appointments = Appointments.ID  ";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        MedicalcardList.Add(new Medical_card()
                        {
                            ID = reader.GetInt32("id"),
                            Patient = reader.GetString("LastName"),
                            Appointments = reader.GetInt32("Appointments"),
                        });
                    }
                }

            }

            connection.Close();
        }

        Medical_cardDataGrid.ItemsSource = MedicalcardList;
    }

    private void Bt_add_OnClick(object? sender, RoutedEventArgs e)
    {
        Add_window addWindow = new Add_window(this);
        addWindow.Show();
    }

    private void Update_bt_OnClick(object? sender, RoutedEventArgs e)
    {
        UpdateAppointments();
    }

    private void Patient_add_filter_OnClick(object? sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(BoxPatients.Text))
        {
            var box = MessageBoxManager.GetMessageBoxStandard("Ошибка!", "Пустой поисковый запрос", ButtonEnum.Ok);
            box.ShowAsync();
            return;
        }

        AppointmentDataGrid.SelectedItems.Clear();
        var foundItems = AppointmentsList.Where(s => s.Patient.Contains(BoxPatients.Text));
        
        foreach (var found in foundItems)
        {
            AppointmentDataGrid.SelectedItems.Add(found);
        }
    }
    
    private void DiagnosisFilterBox_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        var selectedItem = DiagnosisFilterBox.SelectedItem;
        if (selectedItem == null)
        {
            return;
        }

        var foundDiagnosis = AppointmentsList.Where(s => s.Diagnosis == (selectedItem as Diagnosis).Name_Diagnosis);
        AppointmentDataGrid.ItemsSource = foundDiagnosis;
    }

    private void Bt_med_card_OnClick(object? sender, RoutedEventArgs e)
    {
        Add_med_card addWindoww = new Add_med_card(this);
        addWindoww.Show();
    }
}