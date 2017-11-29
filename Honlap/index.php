<!DOCTYPE html>
<html>
    <head>
        <meta charset="UTF-8" lang="hu">
        <title></title>
    </head>
    <body>
        <?php
        include 'dbconnect.php';
        session_start();
        ?>
        <?php
        $conn = MySQLServerConnecter();
        $select = "SELECT kep, termeknev, leiras, keszleten, ar FROM termekek";
        $s = $conn->query($select);
        $conn->close();
        
        if ($_SESSION["login"]!="TRUE" || $_SESSION["login"]!="FALSE") {
            $_SESSION["login"]="VALAMI";
        }
        
        if ($_SESSION["login"] == "TRUE") {
            echo "<p><a href=./logout.php>Kijelentkezés</a></p>";
            
        } else {
            echo "<p><a href=./login.php>Bejelentkezés</a></p>";
            echo "<p><a href=#valami>Regisztráció</a></p>";

            echo "<table>";
            echo "<tr>"
            . "<th>Fénykép</th>"
            . "<th>Terméknév</th>"
            . "<th>Leírás</th>"
            . "<th>Készleten</th>"
            . "<th>Ár</th>"
            . "</tr>";
            while ($row = $s->fetch_assoc()) {
                echo "<tr>"
                . "<td><img src=./image/" . $row["kep"] . " alt=fenykep style=width:128px;height:128px;></td>"
                . "<td>" . $row["termeknev"] . "</td>"
                . "<td>" . $row["leiras"] . "</td>";
                if ($row["keszleten"] == 0)
                    echo "<td>Nincs készleten</td>";
                else
                    echo "<td>Van készleten</td>";
                echo "<td>" . $row["ar"] . " Ft</td>"
                . "</tr>";
            }
            echo "</table>";
        }
        ?>
    </body>
</html>
