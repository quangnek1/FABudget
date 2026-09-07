pipeline {
    agent any
    stages {
        // 1. Các bước chung: Chạy trên TẤT CẢ các nhánh. ok chạy test
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

        // 2. Các bước riêng cho nhánh DEVELOP
        stage('Deploy Staging') {
            when {
                branch 'develop'
            }
            steps {
                echo 'Đang chạy các lệnh deploy cho môi trường Test/Staging...'
                // Ví dụ: bat 'dotnet publish -c Release -o ./publish/staging' ok tesst
            }
        }

        // 3. Các bước riêng cho nhánh MAIN
        stage('Backup Production') {
            when {
                anyOf {
                    branch 'main'
                    branch 'master'
                }
            }
            steps {
                echo 'Đang backup dữ liệu trước khi lên Production...'
                // Lệnh thực thi backup của bạn
            }
        }
        
        stage('Deploy Production') {
            when {
                anyOf {
                    branch 'main'
                    branch 'master'
                }
            }
            steps {
                echo 'Đang deploy phiên bản mới lên Production...'
                // Ví dụ: bat 'dotnet publish -c Release -o ./publish/production'
            }
        }
    }
}
