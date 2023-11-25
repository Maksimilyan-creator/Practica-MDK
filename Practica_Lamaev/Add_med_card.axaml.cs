using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using MySqlConnector;
using Practica_Lamaev.Models;

namespace Practica_Lamaev;

public partial class Add_med_card : Window
{
    private window mywin;
    private MySqlConnectionStringBuilder _connectionSb;
    private List<Appointments> newAppointmentsList { get; set; }
    private List<Patients> newPatientsList { get; set; }
    private Medical_card NewMedicalCards;
    private List<Medical_card> newMedicalCardsList { get; set; }
    private List<Medical_card> MedicalCardsList { get; set; }

    public Add_med_card( window win)
    {
        InitializeComponent();
        mywin = win;
        newMedicalCardsList = new List<Medical_card>();
        MedicalCardsList = new List<Medical_card>();
        newPatientsList = new List<Patients>();
        newAppointmentsList = new List<Appointments>();
        NewMedicalCards = new Medical_card();
        DataContext = NewMedicalCards ;
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
        UpdatePatients();
        UpdateAppointments();
#if DEBUG
        this.AttachDevTools();
#endif
        
        PatientsComboBox = this.Find<ComboBox>("PatientsComboBox");
        AppointmentsComboBox = this.Find<ComboBox>("AppointmentsComboBox");
    }
    void UpdatePatients()
        {
        
            using (var connection = new MySqlConnection(_connectionSb.ConnectionString))
            {
                newPatientsList.Clear();
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT * From Patients";
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            newPatientsList.Add(new Patients()
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
            PatientsComboBox.ItemsSource = newPatientsList;
        }
        public void UpdateAppointments()
        {
            newAppointmentsList.Clear();
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
                            newAppointmentsList.Add(new Appointments()
                            {
                                ID = reader.GetInt32("ID"),
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

            AppointmentsComboBox.ItemsSource = newAppointmentsList;
        }

        private void Bt_add_OnClick(object? sender, RoutedEventArgs e)
        {
            using (var c = new MySqlConnection(_connectionSb.ConnectionString))
            {
                c.Open();
                using (var cmd = c.CreateCommand())
                {
                    cmd.CommandText =
                        "INSERT INTO Medical_card (Patient, Appointments )" +
                        "VALUES (@Patient, @Appointments)";

                    cmd.Parameters.AddWithValue("@Patient", (( PatientsComboBox.SelectedItem) as Patients).ID);
                    cmd.Parameters.AddWithValue("@Appoint", ((AppointmentsComboBox.SelectedItem) as Appointments).ID);
                    var rowsCount = cmd.ExecuteNonQuery();
                    if (rowsCount == 0) ;
                    {
                        var box = MessageBoxManager.GetMessageBoxStandard("Успех", "Удалось выполнить добавление!",
                            ButtonEnum.Ok);
                        box.ShowAsync();
                    }
                }
                c.Close();
            }

            mywin.UpdateMedical_card();
            this.Close();
        }
}