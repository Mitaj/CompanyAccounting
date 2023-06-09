PGDMP     -                    {            company_accounting    15.2    15.2 '    &           0    0    ENCODING    ENCODING        SET client_encoding = 'UTF8';
                      false            '           0    0 
   STDSTRINGS 
   STDSTRINGS     (   SET standard_conforming_strings = 'on';
                      false            (           0    0 
   SEARCHPATH 
   SEARCHPATH     8   SELECT pg_catalog.set_config('search_path', '', false);
                      false            )           1262    16562    company_accounting    DATABASE     �   CREATE DATABASE company_accounting WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE_PROVIDER = libc LOCALE = 'Russian_Russia.1251';
 "   DROP DATABASE company_accounting;
                postgres    false            *           0    0    DATABASE company_accounting    COMMENT     P   COMMENT ON DATABASE company_accounting IS 'Companies, Divisions and Employees';
                   postgres    false    3369            �            1259    16573 	   Companies    TABLE     �   CREATE TABLE public."Companies" (
    "ID" integer NOT NULL,
    "DateCreation" date,
    "Name" character varying(64) COLLATE pg_catalog."C",
    "LegalAddress" character varying(128)
);
    DROP TABLE public."Companies";
       public         heap    postgres    false            �            1259    16642    Companies_ID_seq    SEQUENCE     �   ALTER TABLE public."Companies" ALTER COLUMN "ID" ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public."Companies_ID_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    214            �            1259    16580    Departments    TABLE     �   CREATE TABLE public."Departments" (
    "ID" integer NOT NULL,
    "SupervisorID" integer,
    "Name" character varying(64),
    "CompanyID" integer
);
 !   DROP TABLE public."Departments";
       public         heap    postgres    false            �            1259    16647    Departments_ID_seq    SEQUENCE     �   ALTER TABLE public."Departments" ALTER COLUMN "ID" ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public."Departments_ID_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    215            �            1259    16587 	   Employees    TABLE     �   CREATE TABLE public."Employees" (
    "ID" integer NOT NULL,
    "FirstName" character varying(64) NOT NULL,
    "SecondName" character varying(64),
    "LastName" character varying(64) NOT NULL,
    "Birthday" date
);
    DROP TABLE public."Employees";
       public         heap    postgres    false            �            1259    16648    Employees_ID_seq    SEQUENCE     �   ALTER TABLE public."Employees" ALTER COLUMN "ID" ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public."Employees_ID_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    216            �            1259    16599    JobInformations    TABLE     �   CREATE TABLE public."JobInformations" (
    "ID" integer NOT NULL,
    "SalaryDollars" integer NOT NULL,
    "PositionName" character varying(64) NOT NULL
);
 %   DROP TABLE public."JobInformations";
       public         heap    postgres    false            �            1259    16649    JobInformation_ID_seq    SEQUENCE     �   ALTER TABLE public."JobInformations" ALTER COLUMN "ID" ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public."JobInformation_ID_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    218            �            1259    16594    WorkbookEntries    TABLE     �   CREATE TABLE public."WorkbookEntries" (
    "ID" integer NOT NULL,
    "EmployeeID" integer,
    "DepartmentID" integer,
    "JobInformationID" integer,
    "DateEmployment" date
);
 %   DROP TABLE public."WorkbookEntries";
       public         heap    postgres    false            �            1259    16650    WorkbookEntries_ID_seq    SEQUENCE     �   ALTER TABLE public."WorkbookEntries" ALTER COLUMN "ID" ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public."WorkbookEntries_ID_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    217                      0    16573 	   Companies 
   TABLE DATA           S   COPY public."Companies" ("ID", "DateCreation", "Name", "LegalAddress") FROM stdin;
    public          postgres    false    214   !0                 0    16580    Departments 
   TABLE DATA           R   COPY public."Departments" ("ID", "SupervisorID", "Name", "CompanyID") FROM stdin;
    public          postgres    false    215   s0                 0    16587 	   Employees 
   TABLE DATA           ^   COPY public."Employees" ("ID", "FirstName", "SecondName", "LastName", "Birthday") FROM stdin;
    public          postgres    false    216   �0                 0    16599    JobInformations 
   TABLE DATA           R   COPY public."JobInformations" ("ID", "SalaryDollars", "PositionName") FROM stdin;
    public          postgres    false    218   (2                 0    16594    WorkbookEntries 
   TABLE DATA           u   COPY public."WorkbookEntries" ("ID", "EmployeeID", "DepartmentID", "JobInformationID", "DateEmployment") FROM stdin;
    public          postgres    false    217   �2       +           0    0    Companies_ID_seq    SEQUENCE SET     A   SELECT pg_catalog.setval('public."Companies_ID_seq"', 12, true);
          public          postgres    false    219            ,           0    0    Departments_ID_seq    SEQUENCE SET     C   SELECT pg_catalog.setval('public."Departments_ID_seq"', 17, true);
          public          postgres    false    220            -           0    0    Employees_ID_seq    SEQUENCE SET     A   SELECT pg_catalog.setval('public."Employees_ID_seq"', 15, true);
          public          postgres    false    221            .           0    0    JobInformation_ID_seq    SEQUENCE SET     F   SELECT pg_catalog.setval('public."JobInformation_ID_seq"', 14, true);
          public          postgres    false    222            /           0    0    WorkbookEntries_ID_seq    SEQUENCE SET     G   SELECT pg_catalog.setval('public."WorkbookEntries_ID_seq"', 11, true);
          public          postgres    false    223            z           2606    16579    Companies Companies_pid 
   CONSTRAINT     [   ALTER TABLE ONLY public."Companies"
    ADD CONSTRAINT "Companies_pid" PRIMARY KEY ("ID");
 E   ALTER TABLE ONLY public."Companies" DROP CONSTRAINT "Companies_pid";
       public            postgres    false    214            |           2606    16586    Departments Departments_pkey 
   CONSTRAINT     `   ALTER TABLE ONLY public."Departments"
    ADD CONSTRAINT "Departments_pkey" PRIMARY KEY ("ID");
 J   ALTER TABLE ONLY public."Departments" DROP CONSTRAINT "Departments_pkey";
       public            postgres    false    215            �           2606    16593    Employees Employees_pkey 
   CONSTRAINT     \   ALTER TABLE ONLY public."Employees"
    ADD CONSTRAINT "Employees_pkey" PRIMARY KEY ("ID");
 F   ALTER TABLE ONLY public."Employees" DROP CONSTRAINT "Employees_pkey";
       public            postgres    false    216            �           2606    16605 #   JobInformations JobInformation_pkey 
   CONSTRAINT     g   ALTER TABLE ONLY public."JobInformations"
    ADD CONSTRAINT "JobInformation_pkey" PRIMARY KEY ("ID");
 Q   ALTER TABLE ONLY public."JobInformations" DROP CONSTRAINT "JobInformation_pkey";
       public            postgres    false    218            �           2606    16598 $   WorkbookEntries WorkbookEntries_pkey 
   CONSTRAINT     h   ALTER TABLE ONLY public."WorkbookEntries"
    ADD CONSTRAINT "WorkbookEntries_pkey" PRIMARY KEY ("ID");
 R   ALTER TABLE ONLY public."WorkbookEntries" DROP CONSTRAINT "WorkbookEntries_pkey";
       public            postgres    false    217            }           1259    16657 !   fki_Companies_ref_Departments1_fk    INDEX     d   CREATE INDEX "fki_Companies_ref_Departments1_fk" ON public."Departments" USING btree ("CompanyID");
 7   DROP INDEX public."fki_Companies_ref_Departments1_fk";
       public            postgres    false    215            ~           1259    16623    fki_Companies_ref_Employees_fk    INDEX     d   CREATE INDEX "fki_Companies_ref_Employees_fk" ON public."Departments" USING btree ("SupervisorID");
 4   DROP INDEX public."fki_Companies_ref_Employees_fk";
       public            postgres    false    215            �           1259    16635 &   fki_Departments_ref_WorkbookEntries_fk    INDEX     p   CREATE INDEX "fki_Departments_ref_WorkbookEntries_fk" ON public."WorkbookEntries" USING btree ("DepartmentID");
 <   DROP INDEX public."fki_Departments_ref_WorkbookEntries_fk";
       public            postgres    false    217            �           1259    16629 $   fki_Employees_ref_WorkbookEntries_fk    INDEX     l   CREATE INDEX "fki_Employees_ref_WorkbookEntries_fk" ON public."WorkbookEntries" USING btree ("EmployeeID");
 :   DROP INDEX public."fki_Employees_ref_WorkbookEntries_fk";
       public            postgres    false    217            �           1259    16641 )   fki_JobInformation_ref_WorkbookEntries_fk    INDEX     w   CREATE INDEX "fki_JobInformation_ref_WorkbookEntries_fk" ON public."WorkbookEntries" USING btree ("JobInformationID");
 ?   DROP INDEX public."fki_JobInformation_ref_WorkbookEntries_fk";
       public            postgres    false    217            �           2606    16658 (   Departments Companies_ref_Departments_fk    FK CONSTRAINT     �   ALTER TABLE ONLY public."Departments"
    ADD CONSTRAINT "Companies_ref_Departments_fk" FOREIGN KEY ("CompanyID") REFERENCES public."Companies"("ID") MATCH FULL ON UPDATE CASCADE ON DELETE CASCADE;
 V   ALTER TABLE ONLY public."Departments" DROP CONSTRAINT "Companies_ref_Departments_fk";
       public          postgres    false    215    3194    214            �           2606    16630 2   WorkbookEntries Departments_ref_WorkbookEntries_fk    FK CONSTRAINT     �   ALTER TABLE ONLY public."WorkbookEntries"
    ADD CONSTRAINT "Departments_ref_WorkbookEntries_fk" FOREIGN KEY ("DepartmentID") REFERENCES public."Departments"("ID") MATCH FULL ON UPDATE CASCADE ON DELETE CASCADE;
 `   ALTER TABLE ONLY public."WorkbookEntries" DROP CONSTRAINT "Departments_ref_WorkbookEntries_fk";
       public          postgres    false    217    3196    215            �           2606    16624 0   WorkbookEntries Employees_ref_WorkbookEntries_fk    FK CONSTRAINT     �   ALTER TABLE ONLY public."WorkbookEntries"
    ADD CONSTRAINT "Employees_ref_WorkbookEntries_fk" FOREIGN KEY ("EmployeeID") REFERENCES public."Employees"("ID") MATCH FULL ON UPDATE CASCADE ON DELETE CASCADE;
 ^   ALTER TABLE ONLY public."WorkbookEntries" DROP CONSTRAINT "Employees_ref_WorkbookEntries_fk";
       public          postgres    false    217    3200    216            �           2606    16636 5   WorkbookEntries JobInformation_ref_WorkbookEntries_fk    FK CONSTRAINT     �   ALTER TABLE ONLY public."WorkbookEntries"
    ADD CONSTRAINT "JobInformation_ref_WorkbookEntries_fk" FOREIGN KEY ("JobInformationID") REFERENCES public."JobInformations"("ID") MATCH FULL ON UPDATE CASCADE ON DELETE RESTRICT;
 c   ALTER TABLE ONLY public."WorkbookEntries" DROP CONSTRAINT "JobInformation_ref_WorkbookEntries_fk";
       public          postgres    false    218    3207    217               B   x�3�4��4�50�5��t�M������2�4200�50"ΐ��D�����bϼd��=... ���         R   x�3�4�t�M��ϳR�0���.l���b���
�]l���b#��!�!gHjqN"nE�\�朆Ɯ^�V��/F��� ��0Z         C  x��RKN�0]��b�q��/�	��z�^�-�� �EĂ})*����7�MRh�C�����7�&YI+{�htI�$�4��8�l]i}e2�g ����:��B:����u^��p�Zv=~%d:T�(2�of<ɣ|�6-ӒH��%�4�ֱeor��'*2����2��`
%�N�4C��Cc�u��򯁳N �>om�d6cS{b٩R��S�Q�f�QYQ�#����u��h�VU�N�t#���K���HEФ+$�R?:vթ��U�P���vp�#�A��a�Eg�S;�0���A_v���<9�[?�AT7� ��/�����q&         �   x�}OK
�P\'����l��a�ŝw.\xQ[�R�0�Fƾg�"���L�2��<��U�G�Ѣ�{6$�"�~6��)4��[v$UE8%@��{�ʜH�ks�59�}�p�"S�U:75,I�r�M�ᆋ>X�7T�\��ug݉�.�)�|�b�7��6         _   x�%���0B��.� �����sH��O^(�(�L�B��x�\�bh4:�����̦��L���@[iw��2�O����݁M��\~%����     