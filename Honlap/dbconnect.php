            <?php
            function MySQLServerConnecter() {
                $servername = "sql11.freemysqlhosting.net:3306";
                $dbname= "sql11202526"; //régi!!!!!
                $username = "sql11202526";                 
                $password = "pJYsZqIuFs";                 
                $conn = new mysqli($servername, $username, $password, $dbname);  
                
                if ($conn->connect_error) {
                    die("Connection failed: " . $conn->connect_error); 
                } else {
                    return $conn; 
                }
            }