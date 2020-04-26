@echo off

rem ========================================================================
rem 【日付取得ライブラリ】
rem 以下の変数に各種日付を設定する。
rem ・TODAY		：当日
rem ・YESTERDAY	：前日
rem ========================================================================

rem 日付取得準備
set yyyy=%date:~0,4%
set mm=%date:~5,2%
set dd=%date:~8,2%

rem 当日取得
set yy=%yyyy:~-2%
set TODAY=%yyyy%%mm%%dd%

rem 前日取得
set /a dd=%dd%-1
set dd=00%dd%
set dd=%dd:~-2%
set /a ymod=%yy% %% 4
if %dd%==00 (
if %mm%==01 (set mm=12&& set dd=31&& set /a yyyy=%yyyy%-1)
if %mm%==02 (set mm=01&& set dd=31)
if %mm%==03 (set mm=02&& set dd=28&& if %ymod%==0 (set dd=29))
if %mm%==04 (set mm=03&& set dd=31)
if %mm%==05 (set mm=04&& set dd=30)
if %mm%==06 (set mm=05&& set dd=31)
if %mm%==07 (set mm=06&& set dd=30)
if %mm%==08 (set mm=07&& set dd=31)
if %mm%==09 (set mm=08&& set dd=31)
if %mm%==10 (set mm=09&& set dd=30)
if %mm%==11 (set mm=10&& set dd=31)
if %mm%==12 (set mm=11&& set dd=30)
)

set yy=%yyyy:~-2%
set YESTERDAY=%yyyy%%mm%%dd%

exit /b
