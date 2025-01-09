using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KretaBasicSchoolSystem.Desktop.Models;
using KretaBasicSchoolSystem.Desktop.Service;
using KretaBasicSchoolSystem.Desktop.ViewModels.Base;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace KretaBasicSchoolSystem.Desktop.ViewModels.SchoolCitizens
{
    public partial class StudentViewModel : BaseViewModelWithAsyncInitialization
    {        
        private readonly IStudentService? _studentService;

        [ObservableProperty]
        private ObservableCollection<string> _educationLevels = new ObservableCollection<string>(new EducationLevels().AllEducationLevels);

        [ObservableProperty]
        private ObservableCollection<Student> _students = new ObservableCollection<Student>();

        [ObservableProperty]
        private Student _selectedStudent;


        private bool _isBusy=false;
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                SetProperty(ref _isBusy, value);
                _isEnable = !_isBusy;
                OnPropertyChanged(nameof(IsEnable));
            }
        }

        private bool _isEnable = true;
        public bool IsEnable
        {
            get => _isEnable;
        }


        private string _selectedEducationLevel = string.Empty;
        public string SelectedEducationLevel
        {
            get => _selectedEducationLevel;
            set
            {
                SetProperty(ref _selectedEducationLevel, value);
                SelectedStudent.EducationLevel = _selectedEducationLevel;
            }
        }

        public StudentViewModel()
        {
            SelectedStudent = new Student();
            SelectedEducationLevel = _educationLevels[0];
        }

        public StudentViewModel(IStudentService? studentService)
        {
            //Students.Add(new Student("Elek", "Teszt", System.DateTime.Now, 9, SchoolClassType.ClassA, ""));
            SelectedStudent = new Student();
            SelectedEducationLevel = _educationLevels[0];

            _studentService = studentService;
        }

        [RelayCommand]
        private void DoSave(Student newStudent)
        {
        }

        [RelayCommand]
        private async Task DoNewStudent()
        {
        }

        [RelayCommand]
        private async Task DoRemove(Student studentToDelete)
        {

        }

        public override async Task InitializeAsync()
        {
            if (_studentService is not null)
            {
                IsBusy = true; Students = new ObservableCollection<Student>();
                List<Student> students = await _studentService.SelectAllStudent();
                Students = new ObservableCollection<Student>(students);
                IsBusy = false;
            }
        }
    }
}
