pipeline {
    agent any
    stages {
        stage('Restore') {
            steps {
                bat 'dotnet restore'
                // ok
            }
        }
        stage('Build') {
            steps {
                bat 'dotnet build --no-restore'
            }
        }
    }
}
