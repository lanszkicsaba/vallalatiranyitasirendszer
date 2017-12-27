<html>
    <?php
    session_start();
    require_once 'dbconnect.php';
    ?>
    <script>


    </script>
    <body>
        <form action=checkout.php method=post>
            <table>
                <thead>
                    <tr>
                        <th>Kép:</th>
                        <th>Név:</th>
                        <th>Ár:</th>
                        <th>Darabszám:</th>
                    </tr>
                </thead>
                <tbody>
                    A kosár tartalma:
                    <?php
//unset($_SESSION['cart2']);
                    if(count($_POST)>0) $termekid = $_POST['id'];
                    else $termekid=0;
                    $conn = MySQLServerConnecter();
                    mysqli_set_charset($conn, "utf8");

                    if (!isset($_SESSION['cart2'])) {
                        $_SESSION['cart2'] = array();
                    }
                    if (!array_key_exists($termekid, $_SESSION['cart2'])) {
                        array_push($_SESSION['cart2'], $termekid);
                    }

                    foreach ($_SESSION['cart2'] as $key => $value) {
                        $query = "SELECT kep, termeknev,ar FROM termekek WHERE id=" . $value;
                        $result = mysqli_query($conn, $query) or die("Nem sikerült" . $query);

                        while (list($kep, $termeknev, $termekar) = mysqli_fetch_row($result)) {
                            echo '<tr>
				<td><img src=./image/' . $kep . ' alt=fenykep style=width:50px;height:50px;></td>
				<td>' . $termeknev . '</td>		
				<td>' . $termekar . '</td>
				<td>Darabszám:<input type="text" name="darabszam" size="3" value="1"/></td>
				</tr>';
                        }
                        //echo $value;
                    }
                    mysqli_close($conn);
                    echo '
	 <tr></tr>
	 <tr>
	 <td colspan="4">
	 
	 </td>
	 </tr>
	 <tr>
	 <td colspan="4"><input type="button" onClick="parent.location=\'index.php\'" value="Vissza a vásárláshoz"></input>
	 <input type="submit" value="Tovább a fizetéshez"></input></td>
	 </tr>
	 ';
                    ?>
                </tbody>
            </table>

        </form>
    </body>
</html>