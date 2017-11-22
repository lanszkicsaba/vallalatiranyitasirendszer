<!DOCTYPE html>
<html>
    <head>
        <meta charset="UTF-8">
        <title></title>
    </head>
    <body>
        <?php include 'dbconnect.php'; ?>
        <?php
        $conn = MySQLServerConnecter();
        $select = "SELECT kep, termeknev, leiras, keszleten, ar FROM termekek";
        $s = $conn->query($select);
        $conn->close();

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
            . "<td>" . $row["leiras"] . "</td>"
            . "<td>" . $row["keszleten"] . "</td>"
            . "<td>" . $row["ar"] . "</td>"
            . "</tr>";
        }
        echo "</table>";
        ?>
    </body>
</html>
