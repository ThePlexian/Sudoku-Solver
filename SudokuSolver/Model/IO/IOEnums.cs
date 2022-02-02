
namespace SudokuSolver.Model.IO 
{
    public enum FileAccess { 
        CreateOnly, 
        CreateOrOverwrite, 
        CreateOrAppend 
    }

    public enum LoadingProcessResult { 
        Success, 
        InvalidFileContent 
    }

    public enum SavingProcessResult { 
        Success, 
        FileAlreadyExists, 
        UnauthorizedAccess 
    }
}
