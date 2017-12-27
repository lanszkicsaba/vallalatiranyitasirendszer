            <?php
            function MySQLServerConnecter() {
                $servername = "sql11.freesqldatabase.com:3306";
                $dbname= "sql11212773"; 
                $username = "sql11212773";                 
                $password = "sJ36t24fes";                 
                $conn = new mysqli($servername, $username, $password, $dbname);  
                
                if ($conn->connect_error) {
                    die("Connection failed: " . $conn->connect_error); 
                } else {
                    return $conn; 
                }
            }