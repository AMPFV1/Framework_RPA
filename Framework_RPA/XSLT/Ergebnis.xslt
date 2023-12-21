<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
>
	<xsl:output encoding="utf-8" method="html" version="4.0" indent="no"/>

	<xsl:template match="/">
		<xsl:comment> saved from url=(0016)http://localhost </xsl:comment>
		<xsl:text>&#xD;&#xA;</xsl:text>
		<html lang="de">
			<head>
				<title>Testreport</title>
			</head>
			<body onload="setChart();">
				<div>
					<div>
						<input type="button" value="zurück" onclick="javascript: history.back();"/>
					</div>
					<div style="margin: 20px; font-size: 24px; font-weight: bold; width: 100%;">
					<div style="text-align: center;">
						<xsl:value-of select="SingleResult/Bereich"/>
					</div>
					<div style="text-align: center;">
						<xsl:value-of select="SingleResult/Bezeichnung"/>
					</div>
					</div>
				</div>
			<style>
					body {
						font-size: 18px;
					}

					table {
						border-collapse: collapse;
					}

					th {
						background-color: #6aff80;
						font-weight: bold;
						color: #000000;
						border: 1px solid black;
					}

					td {
						text-align: center;
						border: 1px solid black;
					}

					.td_OK {
						background-color: #8AFA76;
						text-align: center;
					}

					.td_FAIL {
						background-color: #FF5258;
						text-align: center;
					}

					.td_Error {
						background-color: #808080;
						text-align: center;
					}

					textarea {
						resize: none;
						border: none;
						outline: none;
					}
				</style>
				<xsl:apply-templates select="SingleResult"/>
			</body>
		</html>
	</xsl:template>
	<xsl:template match="SingleResult">
		<div style="display: flex;">
			<div style="width: 100%;">
				<div style="display: flex; align-items: center;">
					<xsl:call-template name="Dauer"/>
				</div>
				<div style="display: flex; margin-bottom: 50px;">
					<xsl:apply-templates select="Ergebnisse"/>
					<xsl:apply-templates select="Fortschritt"/>
				</div>
			</div>
		</div>
	</xsl:template>
	<xsl:template match="Fortschritt">
		<div style="margin-left: 10px; width: 40%;">
			<table style="margin-left: 10px; width: 100%;">
				<caption style="font-weight: bold;font-size: x-large;">Protokoll</caption>
				<tr>
					<th>TimeStamp</th>
					<th>Hinweis</th>
				</tr>
				<xsl:for-each select="Step">
					<tr>
						<td>
							<xsl:value-of select="@TimeStamp"/>
						</td>
						<td>
							<xsl:value-of select="."/>
						</td>
					</tr>
				</xsl:for-each>
			</table>
		</div>
	</xsl:template>
	<xsl:template match="Ergebnisse">
		<div style="margin-left: 50px; width: 40%;">
			<table style="margin-left: 10px; width: 100%;">
				<caption style="font-weight: bold;font-size: x-large;">Ergebnisse</caption>
				<tr>
					<th>TimeStamp</th>
					<th>Hinweis</th>
					<th>Soll</th>
					<th>Ist</th>
					<th>Ergebnis</th>
				</tr>
				<xsl:for-each select="Ergebnis">
					<tr>
						<td>
							<xsl:value-of select="@TimeStamp"/>
						</td>
						<td>
							<xsl:value-of select="."/>
						</td>
						<td>
							<xsl:value-of select="@Soll"/>
						</td>
						<td>
							<xsl:value-of select="@Ist"/>
						</td>
						<xsl:element name="td">
							<xsl:choose>
								<xsl:when test="@Result='OK'">
									<xsl:attribute name="class">td_OK</xsl:attribute>
								</xsl:when>
								<xsl:when test="@Result='FAIL'">
									<xsl:attribute name="class">td_FAIL</xsl:attribute>
								</xsl:when>
								<xsl:when test="@Result='Error'">
									<xsl:attribute name="class">td_Error</xsl:attribute>
								</xsl:when>
							</xsl:choose>
							<xsl:value-of select="@Result"/>
						</xsl:element>
					</tr>
				</xsl:for-each>
			</table>
		</div>
	</xsl:template>
	<xsl:template name="Dauer">
		<div>
			<div style="margin: 20px;">
				<div style="display: flex;">
					<div style="width: 40px;">
						Start
					</div>
					<div style="margin-left: 10px;">
						<xsl:value-of select="Start"/>
					</div>
				</div>
				<div style="display: flex;">
					<div style="width: 40px;">
						Ende
					</div>
					<div style="margin-left: 10px;">
						<xsl:value-of select="Ende"/>
					</div>
				</div>
				<div style="display: flex;">
					<div style="width: 40px;">
						Dauer
					</div>
					<div style="margin-left: 10px;">
						<xsl:value-of select="Dauer"/>
					</div>
				</div>
			</div>
		</div>
	</xsl:template>
	<xsl:template name="Ergebnisübersicht">
		<div style="width: 400px; margin-left: 30px;">
			<canvas id="myChart"></canvas>
		</div>
	</xsl:template>
</xsl:stylesheet>
