//LaserBurn
//This code can be used for private or commercial projects but cannot be sold or redistributed without written permission.
//Copyright Nik W. Kraus / Dark Cube Entertainment LLC. 
    #pragma strict
    
    var StartPoint : Transform;
    	
	var LaserDirection = "X";
	var UseBurn = true;
	
	var IgnoreTag = "reflective";
    var UseMousePos = false;	
	
	var Use2D = false;
	var UseLayerMask = false;
	var Layer : LayerMask;
	
	var LaserOn = true;    
	var UseUVPan = true;
	
	var EndFlareOffset = 0.0;
	
	var SourceFlare : LensFlare;
	var EndFlare : LensFlare;

	var AddSourceLight = true;
	var SourceLightRange = .5;
	var AddEndLight = true;
	var EndLightRange = .5;

    var LaserColor : Color = Color(1,1,1,.5);
    var GlareAngle = 155;
    
    var HitSparks : ParticleSystem;
	var SparkUseLaserColor = false;
	var BurnMarks : Transform;
        
    var StartWidth = 0.1;
    var EndWidth = 0.1;
    var LaserDist = 20.0;
	var TexScrollX = -0.1;
	var TexScrollY = 0.1;
	var UVTexScale = Vector2(4,.4);
    
    private var SectionDetail : int = 2;       
    private var lineRenderer : LineRenderer;
    private var ray = Ray(Vector3(0,0,0), Vector3(0,1,0));	
	private var EndPos : Vector3;
	private var hit: RaycastHit;
	private var	hit2D : RaycastHit2D;
	private var SourceLight : GameObject;
	private var EndLight : GameObject;
	private var ViewAngle : float;
	private var LaserDir : Vector3;
	private var rot : Quaternion;
	private var ray2 : Ray2D;
	private var BurnClone : Transform;
	private var CamDistSource : float;
    private var CamDistEnd  : float;
	
	
    @script RequireComponent(LineRenderer)
    
    function Start() {
         lineRenderer = GetComponent(LineRenderer);
         if(lineRenderer.material == "none")
         lineRenderer.GetComponent.<Renderer>().material = new Material (Shader.Find("LaserAdvanced"));
         
         lineRenderer.castShadows = false;
         lineRenderer.receiveShadows = false;
         
         lineRenderer.SetVertexCount(SectionDetail);
         lineRenderer = GetComponent(LineRenderer);
         
         // Make a lights
        if(AddSourceLight){
		StartPoint.gameObject.AddComponent(Light);
		StartPoint.GetComponent.<Light>().intensity = 1.5;
		StartPoint.GetComponent.<Light>().range = .5;
		}
		
		if(AddEndLight){
			if(EndFlare){
				EndFlare.gameObject.AddComponent(Light);
				EndFlare.GetComponent.<Light>().intensity = 1.5;
				EndFlare.GetComponent.<Light>().range = 1.0;		
			}
			else{Debug.Log("To use End Light, please assign an End Flare");}
		}		
		
		if(LaserDirection=="x" || LaserDirection=="y" || LaserDirection=="z" || LaserDirection=="X" || LaserDirection=="Y" || LaserDirection=="Z"){     
        }
        else{
        	Debug.Log("Laser Direction can only be X, Y or Z");
        	}      
		var Marks = transform.Find("Marks");
		
     }//end start
         
    
        /////////////////////////////////////
    function Update() {
    	if(LaserDirection=="x" || LaserDirection=="X"){		
        	LaserDirection="X";
        	LaserDir = StartPoint.right;
        }
        else if(LaserDirection=="y" || LaserDirection=="Y"){
        	LaserDirection="Y";
        	LaserDir = StartPoint.up;
        }
        else if(LaserDirection=="z" || LaserDirection=="Z"){
        	LaserDirection="Z";
        	LaserDir = StartPoint.forward;
        }
        else{
        LaserDir = StartPoint.forward;
        }        
    	
    	if(Camera.main){
    		CamDistSource = Vector3.Distance(StartPoint.position, Camera.main.transform.position);
    		CamDistEnd = Vector3.Distance(EndPos, Camera.main.transform.position);
    		ViewAngle = Vector3.Angle(LaserDir, Camera.main.transform.forward);		
    	}
    	else{
    		CamDistSource = 5;
    		CamDistEnd = 5;
    		ViewAngle = 55;
    		Debug.Log("Main camera not tagged, please tag main camera");
    	}
		
				 
    	    	  	    	
      if(LaserOn){
      	lineRenderer.enabled = true;               
        lineRenderer.SetWidth(StartWidth,EndWidth);
        lineRenderer.material.color = LaserColor;
        
        //Flare Control
        if(SourceFlare){
        SourceFlare.color = LaserColor;
        SourceFlare.transform.position = StartPoint.position;
        
        if(ViewAngle > GlareAngle && CamDistSource < 20 && CamDistSource > 0){
        SourceFlare.brightness = Mathf.Lerp(SourceFlare.brightness,20.0,.001);	
        }
        else{
        SourceFlare.brightness = Mathf.Lerp(SourceFlare.brightness,0.1,.05);
         }        
        }
        
        if(EndFlare){
        EndFlare.color = LaserColor;
        
        if(CamDistEnd > 20){
        EndFlare.brightness = Mathf.Lerp(EndFlare.brightness,0.0,.1);
        }
        else{
        EndFlare.brightness = Mathf.Lerp(EndFlare.brightness,5.0,.1);
         }
        }// end flare        
        
        //Light Control
        if(AddSourceLight){
        StartPoint.GetComponent.<Light>().color = LaserColor;
        StartPoint.GetComponent.<Light>().range = SourceLightRange;		
		}

        if(AddEndLight){
		 EndFlare.GetComponent.<Light>().range = EndLightRange;
         if(EndFlare){
         EndFlare.GetComponent.<Light>().color = LaserColor;
         }
        }               
        
                        
        /////////////////////Ray Hit
       if(Use2D){
       	var CamMousePos2D = Camera.main.ScreenToWorldPoint(Input.mousePosition);
       	var Dist2D = Vector2.Distance(Vector2(StartPoint.position.x,StartPoint.position.y), Vector2(CamMousePos2D.x,CamMousePos2D.y));
       	
       	if(UseMousePos){
       		if(UseLayerMask){       		
       			hit2D = Physics2D.Raycast(StartPoint.position, CamMousePos2D-StartPoint.position, Dist2D, Layer);
       		}
       		else{
       			hit2D = Physics2D.Raycast(StartPoint.position, CamMousePos2D-StartPoint.position, Dist2D);
       		}
        }
        else{
        	if(UseLayerMask){
        		hit2D = Physics2D.Raycast(StartPoint.position, LaserDir, LaserDist, Layer);
        	}
        	else{
        		hit2D = Physics2D.Raycast(StartPoint.position, LaserDir, LaserDist);
        	}
        }
        
        var ray2 = new Ray2D(StartPoint.position, LaserDir);
        if (hit2D){		        
		       EndPos = hit2D.point;	 		    
			    
			   if(EndFlare){
			   EndFlare.enabled = true;
			    
			   if(AddEndLight){
			   if(EndFlare){
			   EndFlare.GetComponent.<Light>().enabled = true;		    
		     		}
		    	}
		      		    
		    if(EndFlareOffset > 0)
		    	EndFlare.transform.position = hit2D.point + hit2D.normal * EndFlareOffset;
		    else
		    	EndFlare.transform.position = EndPos;
		    }		    
		    ////////////Burn & Sparks
	     if(UseBurn){
	       if(HitSparks && hit2D.transform.gameObject.GetComponent.<Collider2D>().tag != IgnoreTag){	       	       	
	       	HitSparks.GetComponent.<ParticleSystem>().enableEmission  = true;
	       	HitSparks.transform.position = EndPos + hit2D.normal * EndFlareOffset;
	       	HitSparks.transform.rotation = Quaternion.FromToRotation(Vector3.forward, hit2D.normal);
	       	 //////////////Enable Burn Marks
	       	if(BurnMarks){
	       		rot = Quaternion.EulerAngles(0,90,90);
	       		BurnMarks.localScale = Vector3(.5,.5,.5);
	       		BurnClone = Instantiate(BurnMarks, EndPos, rot);
				BurnClone.parent = hit2D.transform;
	       	 }
	       	 //////////Use Laser Color for Sparks
	       	if(SparkUseLaserColor){
	       		  HitSparks.GetComponent.<ParticleSystem>().startColor  = LaserColor;
	       		}
	       	  }
	       	  else{
	       	  	if(HitSparks){	       	       	
	       			HitSparks.GetComponent.<ParticleSystem>().enableEmission  = false;
	       		}
	       	  }	       	  	        
	        }////End Burn & Sparks 
          }
        else{
	        if(EndFlare)
	        EndFlare.enabled = false;	        
	        
	        if(AddEndLight){
		     if(EndFlare){
		     EndFlare.GetComponent.<Light>().enabled = false;
		     }
		    }
		    
		   if(HitSparks){	       	       	
	       			HitSparks.GetComponent.<ParticleSystem>().enableEmission  = false;
	       		}
	       		
		   if(UseMousePos){
		   	EndPos = Vector3(CamMousePos2D.x,CamMousePos2D.y,0);
		   }
		   else{
           	EndPos = ray2.GetPoint(LaserDist);
           }       	        
         }
       	}
       	///3D Ray
       else{
       	if(UseMousePos){
          ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        }
        else{
          ray = new Ray(StartPoint.position, LaserDir); 
        }
       	//////////3D Ray cast
       	if(UseLayerMask){
	        if (Physics.Raycast(ray, hit, LaserDist, Layer)){	        
		        EndPos = hit.point;	 		    
			    
			    if(EndFlare){
			    EndFlare.enabled = true;
			    
			    if(AddEndLight){
			     if(EndFlare){
			     EndFlare.GetComponent.<Light>().enabled = true;		    
			     }
			    }
			      		    
			    if(EndFlareOffset > 0)
			    EndFlare.transform.position = hit.point + hit.normal * EndFlareOffset;
			    else
			    EndFlare.transform.position = EndPos;
			    }		
			    
			  ////////////Burn & Sparks
		     if(UseBurn){
		       if(HitSparks && hit.transform.gameObject.GetComponent.<Collider>().tag != IgnoreTag){	       	       	
		       	HitSparks.GetComponent.<ParticleSystem>().enableEmission  = true;
		       	HitSparks.transform.position = EndPos + hit.normal * .04;
		       	HitSparks.transform.rotation = Quaternion.FromToRotation(Vector3.forward, hit.normal);
		       	 //////////////Enable Burn Marks
		       	if(BurnMarks){
		       		rot = Quaternion.FromToRotation(Vector3.up, hit.normal);
		       		//Instantiate(BurnMarks, EndPos + hit.normal * EndFlareOffset, rot);
		       		BurnClone = Instantiate(BurnMarks, EndPos + hit.normal * EndFlareOffset, rot);
					BurnClone.parent = hit.transform;
		       	 }
		       	 //////////Use Laser Color for Sparks
		       	if(SparkUseLaserColor){
		       		  HitSparks.GetComponent.<ParticleSystem>().startColor  = LaserColor;
		       		}
		       	  }
		       	  else{
		       	  	if(HitSparks){	       	       	
		       			HitSparks.GetComponent.<ParticleSystem>().enableEmission  = false;
		       		}
		       	  }
		       	  	        
		        }////End Burn & Sparks          
	          }///end 3D Ray with Layer mask
	          
	        else{
		        if(EndFlare)
		        EndFlare.enabled = false;	        
		        
		        if(AddEndLight){
			     if(EndFlare){
			     EndFlare.GetComponent.<Light>().enabled = false;
			     }
			    }
			    
	           EndPos = ray.GetPoint(LaserDist);
	           
	           if(HitSparks){	       	       	
		       	HitSparks.GetComponent.<ParticleSystem>().enableEmission  = false;
		       }	        	        	        	        
	         }
	         }
         else{
         	////////Without Layer
         	if (Physics.Raycast(ray, hit, LaserDist)){	        
         	  // print("laser raycast hit object: " + hit.collider.gameObject.name);
		        EndPos = hit.point;	 		    
			    
			    if(EndFlare){
			    EndFlare.enabled = true;
			    
			    if(AddEndLight){
			     if(EndFlare){
			     EndFlare.GetComponent.<Light>().enabled = true;		    
			     }
			    }
			      		    
			    if(EndFlareOffset > 0)
			    EndFlare.transform.position = hit.point + hit.normal * EndFlareOffset;
			    else
			    EndFlare.transform.position = EndPos;
			    }		
			    
			  ////////////Burn & Sparks
		     if(UseBurn){
		       if(HitSparks && hit.transform.gameObject.GetComponent.<Collider>().tag != IgnoreTag){	       	       	
		       	HitSparks.GetComponent.<ParticleSystem>().enableEmission  = true;
		       	HitSparks.transform.position = EndPos + hit.normal * .04;
		       	HitSparks.transform.rotation = Quaternion.FromToRotation(Vector3.forward, hit.normal);
		       	 //////////////Enable Burn Marks
		       	if(BurnMarks){
		       		rot = Quaternion.FromToRotation(Vector3.up, hit.normal);
		       		//Instantiate(BurnMarks, EndPos + hit.normal * EndFlareOffset, rot);
		       		BurnClone = Instantiate(BurnMarks, EndPos + hit.normal * EndFlareOffset, rot);
					BurnClone.parent = hit.transform;
		       	 }
		       	 //////////Use Laser Color for Sparks
		       	if(SparkUseLaserColor){
		       		  HitSparks.GetComponent.<ParticleSystem>().startColor  = LaserColor;
		       		}
		       	  }
		       	  else{
		       	  	if(HitSparks){	       	       	
		       			HitSparks.GetComponent.<ParticleSystem>().enableEmission  = false;
		       		}
		       	  }
		       	  	        
		        }////End Burn & Sparks          
	          }///end 3D ray without layer mask
	          
	        else{
		        if(EndFlare)
		        EndFlare.enabled = false;	        
		        
		        if(AddEndLight){
			     if(EndFlare){
			     EndFlare.GetComponent.<Light>().enabled = false;
			     }
			    }
			    
	           EndPos = ray.GetPoint(LaserDist);
	           
	           if(HitSparks){	       	       	
		       	HitSparks.GetComponent.<ParticleSystem>().enableEmission  = false;
		       }	        	        	        	        
	         }	      
         
         }//end Layer Use
         
        }//end Ray           
        
        
        Debug.DrawLine (StartPoint.position, EndPos, Color.red);
        
          //Find Distance
	      var dist = Vector3.Distance(StartPoint.position, EndPos);
	      
	      //Line Render Positions
	      lineRenderer.SetPosition(0,StartPoint.position);
	      lineRenderer.SetPosition(1,EndPos);
	                  
    //Texture Scroller
    if(UseUVPan){
    lineRenderer.material.SetTextureScale("_Mask", Vector2(dist/UVTexScale.x, UVTexScale.y));
    lineRenderer.material.SetTextureOffset ("_Mask", Vector2(TexScrollX*Time.time, TexScrollY*Time.time));
    }
    
   }
   else{
   	if(HitSparks){
   		HitSparks.GetComponent.<ParticleSystem>().enableEmission  = false;
   	}
    lineRenderer.enabled = false;
    
    if(SourceFlare)
    SourceFlare.enabled = false;
    
    if(EndFlare)
	EndFlare.enabled = false;
	        
	if(AddSourceLight)
	StartPoint.GetComponent.<Light>().enabled = false;
	
	if(AddEndLight)
	 if(EndFlare){
	 EndFlare.GetComponent.<Light>().enabled = false;	
	 }	 
   }//end Laser On   
   
  }//end Update
  
  