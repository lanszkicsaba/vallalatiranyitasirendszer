            <?php
            //kapcsolatothoz létre
            function MySQLServerConnecter() {
                $servername = "193.164.132.164:3306"; //MySQL elérési cím
                $dbname= "csaba";  //adatbázisnév
                $username = "csaba";             //felhasználónév    
                $password = "DPU3wX9HYmGEL8HK";                 //jelszó
                $conn = new mysqli($servername, $username, $password, $dbname); //kapcsolat létrehozása  
                
                if ($conn->connect_error) {
                    //ha hiba van kilövi és hibát dob
                    die("Connection failed: " . $conn->connect_error); 
                } else {
                    //visszaadja a kapcsolatot
                    return $conn; 
                }
            }