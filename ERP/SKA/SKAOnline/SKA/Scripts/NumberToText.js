function GetNumberText(number) {
    var numberText = new Array("", "satu", "dua", "tiga", "empat", "lima", "enam", "tujuh", "delapan", "sembilan", "sepuluh", "sebelas");

    if (number < 12)
        return ' ' + numberText[number];
    else if (number < 20)
        return GetNumberText(number % 10) + " belas";
    else if (number < 100)
        return GetNumberText((number / 10) >> 0) + " puluh" + GetNumberText(number % 10);
    else if (number < 200)
        return " seratus" + GetNumberText(number - 100)
    else if (number < 1000)
        return GetNumberText((number / 100) >> 0) + " ratus" + GetNumberText(number % 100);
    else if (number < 2000)
        return " seribu" + GetNumberText(number - 1000)
    else if (number < 1000000)
        return GetNumberText((number / 1000) >> 0) + " ribu" + GetNumberText(number % 1000);
    else if (number < 1000000000)
        return GetNumberText((number / 1000000) >> 0) + " juta" + GetNumberText(number % 1000000);
    else if (number < 1000000000000)
        return GetNumberText((number / 1000000000) >> 0) + " miliar" + GetNumberText(number % 1000000000);
    else if (number < 1000000000000000)
        return GetNumberText((number / 1000000000000) >> 0) + " triliun" + GetNumberText(number % 1000000000000);

    return '';
}