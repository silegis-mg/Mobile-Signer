<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:lexml="http://www.lexml.gov.br/1.0" xmlns:ns1="http://www.w3.org/1999/xlink" xmlns:ns2="http://www.w3.org/1998/Math/MathML" xmlns:silegis="http://silegis.almg.gov.br/" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:ds="http://www.w3.org/2000/09/xmldsig#">
	<xsl:output indent="yes" omit-xml-declaration="yes"/>
  <xsl:strip-space elements="*"/>
	<xsl:template match="silegis:almg">
		<html>
			<head>
				<meta content="text/html; charset=windows-1252" http-equiv="content-type"/>
				<meta content="width=device-width, user-scalable=no" name="viewport"/>
				<title>Proposição</title>
				<style type="text/css">
					  <xsl:comment>

              @page {
                  margin-left: 3.17cm;
                  margin-right: 3.17cm;
                  margin-top: 3.2cm;
                  margin-bottom: 1.7cm;
              }

              body {
                  font-family: Arial;
                  font-size: 11pt;
              }

              p {
                  text-indent: 1cm; 
                  margin-top: 0.05cm; 
                  margin-bottom: 0.05cm; 
                  line-height: 150%; 
                  text-align: justify;
                  page-break-before: auto;
              }

              h1 {
                  margin-top: 0cm;
                  margin-bottom: 0.3cm;
                  text-transform: uppercase;
                  color: #000000;
                  text-align: center;
              }

              h1.epigrafe {
                  font-family: "Arial", sans-serif;
                  font-size: 11pt;
                  margin-bottom: 0.85cm;
              }

              .ementa {
                  margin-left: 5.8cm; 
                  margin-bottom: 0.85cm;
                  text-indent: 0cm; 
                  margin-top: 0.1cm; 
                  background: transparent; 
                  line-height: 150%; 
                  text-align: justify;
                  page-break-before: auto
              }

              .promulgacao {
                  margin-bottom: 0.85cm;
              }
              
              .vocativo {
                  margin-bottom: 0.85cm;
              }

              .fecho {
                  margin-top: 0.85cm;
              }
              
              h1.titulo, h1.capitulo, h1.secao, h1.subsecao {
                  font-family: "Arial", sans-serif;
                  font-size: 11pt;
                  margin-bottom: 0.85cm;
                  margin-top: 0.85cm;
              }
              
              h1.capitulo {
                  font-weight: normal;
              }
              
              h1.secao, h1.subsecao {
                  text-transform: none;
              }

            </xsl:comment>
				  </style>
			</head>
			<body lang="pt-BR" text="#000000">
				  <xsl:apply-templates/>
			</body>
		</html>
	</xsl:template>

	<xsl:template match="silegis:Requerimento">
		  <!-- INICIO CORPO DO REQUERIMENTO -->
		  <h1 class="epigrafe">
			    <xsl:value-of select="lexml:Epigrafe"/>
		  </h1>
		  <p class="vocativo">
					<xsl:value-of select="silegis:Vocativo"/>
		  </p>
		  <xsl:for-each select="silegis:Texto/lexml:p">
			    <p>
				      <xsl:apply-templates/>
			    </p>
		  </xsl:for-each>
		  <p class="fecho">
					<xsl:apply-templates select="silegis:Fecho"/>
		  </p>
      <xsl:apply-templates select="silegis:Justificacao"/>
		  <!-- FIM CORPO DO REQUERIMENTO -->
	</xsl:template>

	<xsl:template match="silegis:Recurso">
		  <!-- INICIO CORPO DO RECURSO -->
		  <h1 class="epigrafe">
			    <xsl:value-of select="lexml:Epigrafe"/>
		  </h1>
		  <p class="vocativo">
				  <xsl:value-of select="silegis:Vocativo"/>
		  </p>
		  <xsl:for-each select="silegis:Texto/lexml:p">
			    <p>
					    <xsl:apply-templates/>
			    </p>
		  </xsl:for-each>
		  <p class="fecho">
				  <xsl:apply-templates select="silegis:Fecho"/>
		  </p>
		  <!-- FIM CORPO DO RECURSO -->
	</xsl:template>

	<xsl:template match="silegis:Comunicacao | silegis:QuestaoDeOrdem">
		  <!-- INICIO CORPO DA COMUNICACAO -->
		  <h1 class="epigrafe">
			    <xsl:value-of select="silegis:Titulo"/>
		  </h1>
		  <p class="vocativo">
				  <xsl:value-of select="silegis:Vocativo"/>
		  </p>
		  <xsl:for-each select="silegis:Texto/lexml:p">
			    <p>
					   <xsl:apply-templates/>
			    </p>
		  </xsl:for-each>
		  <p class="fecho">
				  <xsl:apply-templates select="silegis:Fecho"/>
		  </p>
		  <!-- FIM CORPO DO COMUNICACAO -->
	</xsl:template>
  
	<xsl:template match="silegis:Projeto">
		  <!-- INICIO CORPO DA PROJETO -->
		  <h1 class="epigrafe">
			    <xsl:value-of select="lexml:Epigrafe"/>
		  </h1>
			
		  <p class="ementa">
			    <xsl:apply-templates select="lexml:Ementa"/>
		  </p>

		  <p class="promulgacao">
				  <xsl:value-of select="lexml:FormulaPromulgacao/lexml:p"/>
      </p>

		  <xsl:apply-templates select="lexml:Articulacao"/>

      <p class="fecho">
          <xsl:apply-templates select="silegis:Fecho"/>
      </p>
    
      <xsl:apply-templates select="silegis:Justificacao"/>
		  <!-- FIM CORPO DO PROJETO -->
	</xsl:template>

  <xsl:template match="silegis:Fecho">
      <xsl:value-of select="silegis:Local"/>
      <xsl:text>, </xsl:text>
      <xsl:apply-templates select="silegis:Data"/>
  </xsl:template>
  
  <xsl:template match="silegis:Data">
    
    <xsl:variable name="dia" select="number(substring(., 9, 2))"></xsl:variable>
    <xsl:variable name="ano" select="substring(., 1, 4)"></xsl:variable>
    
    <xsl:variable name="mes">
      <xsl:call-template name="nomeMes">
        <xsl:with-param name="mes" select="number(substring(., 6, 2))"/>
      </xsl:call-template>
    </xsl:variable>    

    <xsl:value-of select="concat(
                  $dia, 
                  ' de ',
                  $mes, 
                  ' de ',
                  $ano)"/>
  
  </xsl:template>

  <xsl:template name="nomeMes">
    <xsl:param name="mes"/>
    <xsl:choose>
      <xsl:when test="$mes=1">Janeiro</xsl:when>
      <xsl:when test="$mes=2">Fevereiro</xsl:when>
      <xsl:when test="$mes=3">Março</xsl:when>
      <xsl:when test="$mes=4">Abril</xsl:when>
      <xsl:when test="$mes=5">Maio</xsl:when>
      <xsl:when test="$mes=6">Junho</xsl:when>
      <xsl:when test="$mes=7">Julho</xsl:when>
      <xsl:when test="$mes=8">Agosto</xsl:when>
      <xsl:when test="$mes=9">Setembro</xsl:when>
      <xsl:when test="$mes=10">Outubro</xsl:when>
      <xsl:when test="$mes=11">Novembro</xsl:when>
      <xsl:when test="$mes=12">Dezembro</xsl:when>
    </xsl:choose>
  </xsl:template>
    
  <xsl:template match="silegis:Justificacao">
      <p>
        <br/>
      </p>
      <!-- O primeiro parágrafo deve começar com "Justificação:" -->
      <p>
        <b>Justificação:<xsl:text> </xsl:text></b>
        <xsl:value-of select="lexml:p[1]"/>
      </p>
      <xsl:for-each select="lexml:p[position() > 1]">
			    <p>
					   <xsl:value-of select="."/>
			    </p>
		  </xsl:for-each>
  </xsl:template>
  
  <!-- OCULTA SIGNATURE SE EXISTIR-->
  <xsl:template match="ds:Signature">
      
  </xsl:template>
  
  <!-- INICIO ARTICULACAO -->
	<xsl:template match="lexml:Articulacao">
		  <xsl:apply-templates/>
	</xsl:template>	

  <xsl:template match="lexml:Titulo">
		  <h1 class="titulo">
			    <xsl:apply-templates select="lexml:Rotulo"/>
          <br/>
          <xsl:apply-templates select="lexml:NomeAgrupador"/>
		  </h1>
      <xsl:apply-templates select="lexml:Capitulo|lexml:Artigo"/>
  </xsl:template>
  
  <xsl:template match="lexml:Capitulo">
		  <h1 class="capitulo">
			    <xsl:apply-templates select="lexml:Rotulo"/>
          <br/>
          <xsl:apply-templates select="lexml:NomeAgrupador"/>
		  </h1>
      <xsl:apply-templates select="lexml:Secao|lexml:Artigo"/>
  </xsl:template>

  <xsl:template match="lexml:Secao">
		  <h1 class="secao">
			    <xsl:apply-templates select="lexml:Rotulo"/>
          <br/>
          <xsl:apply-templates select="lexml:NomeAgrupador"/>
		  </h1>
      <xsl:apply-templates select="lexml:Subsecao|lexml:Artigo"/>
  </xsl:template>

  <xsl:template match="lexml:Subsecao">
		  <h1 class="subsecao">
			    <xsl:apply-templates select="lexml:Rotulo"/>
          <br/>
          <xsl:apply-templates select="lexml:NomeAgrupador"/>
		  </h1>
      <xsl:apply-templates select="lexml:Artigo"/>
  </xsl:template>
      
	<xsl:template match="lexml:Artigo">
		  <p>
			    <xsl:value-of select="lexml:Rotulo"/>
          <xsl:text> </xsl:text>
          <xsl:apply-templates select="lexml:Caput"/>
		  </p>
      <xsl:apply-templates select="lexml:Paragrafo"/>
  </xsl:template>
  
  <xsl:template match="lexml:p">
      <xsl:apply-templates/>
  </xsl:template>
  
  <xsl:template match="lexml:i">
      <i><xsl:value-of select="."/></i>
  </xsl:template>

  <xsl:template match="lexml:Caput">
      <xsl:apply-templates/>
  </xsl:template>
  
  <xsl:template match="lexml:Rotulo">
      <xsl:value-of select="."/>
      <xsl:text> </xsl:text>
  </xsl:template>

  <xsl:template match="lexml:Paragrafo">
      <p>
          <xsl:apply-templates select="lexml:Rotulo"/>
          <xsl:apply-templates select="lexml:p"/>
      </p>
      <xsl:apply-templates select="lexml:Inciso"/>
  </xsl:template>

  <xsl:template match="lexml:Inciso">
      <p>
          <xsl:apply-templates select="lexml:Rotulo"/>
          <xsl:apply-templates select="lexml:p"/>
      </p>
      <xsl:apply-templates select="lexml:Alinea"/>
  </xsl:template>

  <xsl:template match="lexml:Alinea">
      <p>
          <xsl:apply-templates select="lexml:Rotulo"/>
          <xsl:apply-templates select="lexml:p"/>
      </p>
      <xsl:apply-templates select="lexml:Item"/>
  </xsl:template>

  <xsl:template match="lexml:Item">
      <p>
          <xsl:apply-templates select="lexml:Rotulo"/>
          <xsl:apply-templates select="lexml:p"/>
      </p>
  </xsl:template>
  <!-- FIM ARTICULACAO -->
</xsl:stylesheet>
