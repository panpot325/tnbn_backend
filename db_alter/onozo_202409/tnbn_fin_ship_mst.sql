DROP TABLE IF EXISTS tnbn_fin_ship_mst;
CREATE TABLE tnbn_fin_ship_mst
(
    sno          CHAR(6) NOT NULL, -- 船番6桁を設定
    create_date  INTEGER,          -- 
    create_syain INTEGER,          --
    CONSTRAINT   pk_tnbn_fin_ship_mst PRIMARY KEY (sno)
);

INSERT INTO tnbn_fin_ship_mst (sno, create_date, create_syain)
VALUES ('111   ', 20170124, 2010019),
       ('123   ', 20171016, 2010019),
       ('333   ', 20171016, 2010019),
       ('334   ', 20171016, 2010019),
       ('540   ', 20091224, 2007024),
       ('541   ', 20091224, 2007024),
       ('542   ', 20091224, 2007024),
       ('543   ', 20091224, 2007024),
       ('544   ', 20091224, 2007024),
       ('545   ', 20091224, 2007024),
       ('546   ', 20091224, 2007024),
       ('547   ', 20091224, 2007024),
       ('548   ', 20091224, 2007024),
       ('549   ', 20091225, 2007024),
       ('550   ', 20091225, 2007024),
       ('551   ', 20100113, 1998026),
       ('552   ', 20100129, 1998026),
       ('553   ', 20100714, 1998026),
       ('554   ', 20100714, 1998026),
       ('555   ', 20100714, 1998026),
       ('556   ', 20100806, 1998026),
       ('557   ', 20101125, 1998026),
       ('558   ', 20101125, 1998026),
       ('559   ', 20101125, 1998026),
       ('560   ', 20110111, 1998026),
       ('561   ', 20110721, 1998026),
       ('562   ', 20110721, 1998026),
       ('563   ', 20110721, 1998026),
       ('564   ', 20110913, 1998026),
       ('565   ', 20110721, 1998026),
       ('566   ', 20110721, 1998026),
       ('567   ', 20111107, 1998026),
       ('568   ', 20130821, 2010019),
       ('569   ', 20121210, 1998026),
       ('570   ', 20131220, 2010019),
       ('571   ', 20160824, 2010019),
       ('575   ', 20121210, 1998026),
       ('576   ', 20121210, 1998026),
       ('577   ', 20121210, 1998026),
       ('578   ', 20121210, 1998026),
       ('580   ', 20111117, 1998026),
       ('581   ', 20120307, 1998026),
       ('588   ', 20120307, 1998026),
       ('589   ', 20110913, 1998026),
       ('590   ', 20130208, 2010019),
       ('591   ', 20130208, 2010019),
       ('592   ', 20130218, 2010019),
       ('593   ', 20140509, 2010019),
       ('594   ', 20160614, 2010019),
       ('595   ', 20140509, 2010019),
       ('596   ', 20160824, 2010019),
       ('661   ', 20150528, 2010019),
       ('662   ', 20150528, 2010019),
       ('667   ', 20130326, 2010019),
       ('668   ', 20140509, 2010019),
       ('678   ', 20130725, 2010019),
       ('701   ', 20150528, 2010019),
       ('702   ', 20150528, 2010019),
       ('706   ', 20151226, 2010019),
       ('707   ', 20151226, 2010019),
       ('709   ', 20150528, 2010019),
       ('711   ', 20150528, 2010019),
       ('712   ', 20160823, 2010019),
       ('713   ', 20160823, 2010019),
       ('714   ', 20160823, 2010019),
       ('716   ', 20171016, 2010019),
       ('717   ', 20171016, 2010019),
       ('719   ', 20160823, 2010019),
       ('720   ', 20160823, 2010019),
       ('721   ', 20170124, 2010019),
       ('730   ', 20190418, 2010019),
       ('733   ', 20170124, 2010019),
       ('742   ', 20190418, 2010019),
       ('743   ', 20171016, 2010019),
       ('744   ', 20171016, 2010019),
       ('746   ', 20190418, 2010019),
       ('747   ', 20190418, 2010019),
       ('748   ', 20190418, 2010019),
       ('749   ', 20190418, 2010019),
       ('750   ', 20190418, 2010019),
       ('753   ', 20190418, 2010019),
       ('754   ', 20190418, 2010019),
       ('770   ', 20190809, 2010019),
       ('999   ', 20190418, 2010019),
       ('SYUZEN', 20190920, 2010019)
;